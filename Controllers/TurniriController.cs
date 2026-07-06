using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GamingTurnir.Data;
using GamingTurnir.Models;

// ============================================================
// VEZA BACKEND <-> FRONTEND
// ============================================================
// Ruta ovog kontrolera je: /api/turniri
// Frontend poziva ove endpointe za upravljanje turnirima:
//   GET    /api/turniri        -> lista svih turnira
//   GET    /api/turniri/{id}   -> detalji jednog turnira
//   POST   /api/turniri        -> kreiranje novog turnira (samo Admin)
//   PUT    /api/turniri/{id}   -> izmena turnira (samo Admin)
//   DELETE /api/turniri/{id}   -> brisanje turnira (samo Admin)
//
// Frontend mora slati JWT token u headeru uz svaki zahtev:
//   Authorization: Bearer <token>
// ============================================================

// ============================================================
// ODREDJIVANJE ROLE:
// [Authorize] na klasi = svi prijavljeni korisnici mogu citati.
// Kreiranje, izmena i brisanje turnira su rezervisani samo za Admin.
// - GET:    svi prijavljeni korisnici
// - POST:   samo Admin
// - PUT:    samo Admin
// - DELETE: samo Admin
// ============================================================
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TurniriController : ControllerBase
{
    // Dependancy injection - EF Core kontekst za rad sa bazom podataka
    private readonly ApplicationDbContext _context;

    // Konstruktor prima kontekst kroz DI (konfigurisan u Program.cs)
    public TurniriController(ApplicationDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------
    // GET /api/turniri
    // Vraca listu svih turnira (ID, naziv, igrica, datum pocetka).
    // Dostupno svim prijavljenim korisnicima.
    // Frontend koristi ovo za prikaz liste turnira na pocetnoj strani.
    // -------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var turniri = await _context.Turniri
            .Select(t => new {
                t.TurnirId, t.Naziv, t.Igrica, t.DatumPocetka
            }).ToListAsync();
        return Ok(turniri);
    }

    // -------------------------------------------------------
    // GET /api/turniri/{id}
    // Vraca sve detalje jednog turnira po ID-u.
    // Dostupno svim prijavljenim korisnicima.
    // Vraca 404 ako turnir sa datim ID-em ne postoji.
    // -------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var turnir = await _context.Turniri.FindAsync(id);
        if (turnir == null) return NotFound(new { message = "Turnir nije pronađen." });
        return Ok(turnir);
    }

    // -------------------------------------------------------
    // POST /api/turniri
    // Kreira novi turnir u bazi na osnovu podataka iz tela zahteva.
    // [Authorize(Roles = "Admin")] - SAMO Admin moze da kreira turnir.
    // *** OVDE SE PROVERAVA ROLA: ako korisnik nije Admin,
    //     server vraca HTTP 403 Forbidden bez izvrsavanja koda. ***
    // Vraca 201 Created sa podacima kreiranog turnira.
    // -------------------------------------------------------
    // [ROLA] Samo "Admin" ima pristup ovom endpointu
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Turnir dto)
    {
        var turnir = new Turnir
        {
            Naziv = dto.Naziv,
            Igrica = dto.Igrica,
            // DateTime se konvertuje u UTC da bi se ispravno cuvao u bazi
            DatumPocetka = DateTime.SpecifyKind(dto.DatumPocetka, DateTimeKind.Utc)
        };
        _context.Turniri.Add(turnir);
        await _context.SaveChangesAsync();
        return Created("", new { turnir.TurnirId, turnir.Naziv, turnir.Igrica, turnir.DatumPocetka });
    }

    // -------------------------------------------------------
    // PUT /api/turniri/{id}
    // Azurira postojeci turnir sa novim podacima.
    // [Authorize(Roles = "Admin")] - SAMO Admin moze da menja turnir.
    // *** OVDE SE PROVERAVA ROLA: ako korisnik nije Admin,
    //     server vraca HTTP 403 Forbidden bez izvrsavanja koda. ***
    // Vraca 404 ako turnir ne postoji, ili 204 No Content pri uspehu.
    // -------------------------------------------------------
    // [ROLA] Samo "Admin" ima pristup ovom endpointu
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Turnir dto)
    {
        var turnir = await _context.Turniri.FindAsync(id);
        if (turnir == null) return NotFound(new { message = "Turnir nije pronađen." });

        turnir.Naziv = dto.Naziv;
        turnir.Igrica = dto.Igrica;
        turnir.DatumPocetka = DateTime.SpecifyKind(dto.DatumPocetka, DateTimeKind.Utc);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // -------------------------------------------------------
    // DELETE /api/turniri/{id}
    // Brise turnir iz baze po ID-u.
    // [Authorize(Roles = "Admin")] - SAMO Admin moze da brise turnir.
    // *** OVDE SE PROVERAVA ROLA: ako korisnik nije Admin,
    //     server vraca HTTP 403 Forbidden bez izvrsavanja koda. ***
    // Vraca 404 ako turnir ne postoji, ili 204 No Content pri uspehu.
    // -------------------------------------------------------
    // [ROLA] Samo "Admin" ima pristup ovom endpointu
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var turnir = await _context.Turniri.FindAsync(id);
        if (turnir == null) return NotFound(new { message = "Turnir nije pronađen." });

        _context.Turniri.Remove(turnir);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
