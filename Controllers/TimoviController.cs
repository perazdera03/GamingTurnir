using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GamingTurnir.Data;
using GamingTurnir.Models;

// ============================================================
// VEZA BACKEND <-> FRONTEND
// ============================================================
// Ruta ovog kontrolera je: /api/timovi
// Frontend poziva ove endpointe za upravljanje timovima:
//   GET    /api/timovi        -> lista svih timova
//   GET    /api/timovi/{id}   -> detalji jednog tima
//   POST   /api/timovi        -> kreiranje novog tima (samo Admin)
//   PUT    /api/timovi/{id}   -> izmena tima (Admin ili Kapiten)
//   DELETE /api/timovi/{id}   -> brisanje tima (samo Admin)
//
// Frontend mora slati JWT token u headeru uz svaki zahtev:
//   Authorization: Bearer <token>
// ============================================================

// ============================================================
// ODREDJIVANJE ROLE:
// [Authorize] na klasi = svi prijavljeni korisnici mogu citati.
// Pojedinim endpointima su dodeljene specificne role:
// - GET:    svi prijavljeni korisnici
// - POST:   samo Admin
// - PUT:    Admin ili Kapiten  <-- JEDINI ENDPOINT SA DVE ROLE
// - DELETE: samo Admin
// ============================================================
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TimoviController : ControllerBase
{
    // Dependancy injection - EF Core kontekst za rad sa bazom podataka
    private readonly ApplicationDbContext _context;

    // Konstruktor prima kontekst kroz DI (konfigurisan u Program.cs)
    public TimoviController(ApplicationDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------
    // GET /api/timovi
    // Vraca listu svih timova (ID, naziv, opis, datum osnivanja).
    // Dostupno svim prijavljenim korisnicima.
    // Frontend koristi ovo za prikaz liste timova.
    // -------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var timovi = await _context.Timovi
            .Select(t => new {
                t.TimId, t.Naziv, t.Opis, t.DatumOsnivanja
            }).ToListAsync();
        return Ok(timovi);
    }

    // -------------------------------------------------------
    // GET /api/timovi/{id}
    // Vraca sve detalje jednog tima po ID-u.
    // Dostupno svim prijavljenim korisnicima.
    // Vraca 404 ako tim sa datim ID-em ne postoji.
    // -------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tim = await _context.Timovi.FindAsync(id);
        if (tim == null) return NotFound(new { message = "Tim nije pronađen." });
        return Ok(tim);
    }

    // -------------------------------------------------------
    // POST /api/timovi
    // Kreira novi tim u bazi na osnovu podataka iz tela zahteva.
    // [Authorize(Roles = "Admin")] - SAMO Admin moze da kreira tim.
    // *** OVDE SE PROVERAVA ROLA: ako korisnik nije Admin,
    //     server vraca HTTP 403 Forbidden bez izvrsavanja koda. ***
    // Vraca 201 Created sa podacima kreiranog tima.
    // -------------------------------------------------------
    // [ROLA] Samo "Admin" ima pristup ovom endpointu
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Tim dto)
    {
        var tim = new Tim
        {
            Naziv = dto.Naziv,
            Opis = dto.Opis,
            // DateTime se konvertuje u UTC da bi se ispravno cuvao u bazi
            DatumOsnivanja = DateTime.SpecifyKind(dto.DatumOsnivanja, DateTimeKind.Utc)
        };
        _context.Timovi.Add(tim);
        await _context.SaveChangesAsync();
        return Created("", new { tim.TimId, tim.Naziv, tim.Opis, tim.DatumOsnivanja });
    }

    // -------------------------------------------------------
    // PUT /api/timovi/{id}
    // Azurira postojeci tim sa novim podacima.
    // [Authorize(Roles = "Admin,Kapiten")] - Admin ILI Kapiten mogu da menjaju tim.
    // *** OVDE SE PROVERAVA ROLA: pristup imaju dve razlicite role. ***
    // Vraca 404 ako tim ne postoji, ili 204 No Content pri uspehu.
    // -------------------------------------------------------
    // [ROLA] "Admin" i "Kapiten" imaju pristup ovom endpointu
    [Authorize(Roles = "Admin,Kapiten")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Tim dto)
    {
        var tim = await _context.Timovi.FindAsync(id);
        if (tim == null) return NotFound(new { message = "Tim nije pronađen." });

        tim.Naziv = dto.Naziv;
        tim.Opis = dto.Opis;
        tim.DatumOsnivanja = DateTime.SpecifyKind(dto.DatumOsnivanja, DateTimeKind.Utc);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // -------------------------------------------------------
    // DELETE /api/timovi/{id}
    // Brise tim iz baze po ID-u.
    // [Authorize(Roles = "Admin")] - SAMO Admin moze da brise tim.
    // *** OVDE SE PROVERAVA ROLA: ako korisnik nije Admin,
    //     server vraca HTTP 403 Forbidden bez izvrsavanja koda. ***
    // Vraca 404 ako tim ne postoji, ili 204 No Content pri uspehu.
    // -------------------------------------------------------
    // [ROLA] Samo "Admin" ima pristup ovom endpointu
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tim = await _context.Timovi.FindAsync(id);
        if (tim == null) return NotFound(new { message = "Tim nije pronađen." });

        _context.Timovi.Remove(tim);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
