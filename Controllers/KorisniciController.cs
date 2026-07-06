using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GamingTurnir.Data;
using GamingTurnir.Models;

// ============================================================
// VEZA BACKEND <-> FRONTEND
// ============================================================
// Ruta ovog kontrolera je: /api/korisnici
// Frontend poziva ove endpointe za upravljanje korisnicima:
//   GET    /api/korisnici        -> lista svih korisnika
//   GET    /api/korisnici/{id}   -> detalji jednog korisnika
//   PUT    /api/korisnici/{id}   -> izmena korisnika (username, rola)
//   DELETE /api/korisnici/{id}   -> brisanje korisnika
//
// Frontend mora slati JWT token u headeru uz svaki zahtev:
//   Authorization: Bearer <token>
// ============================================================

// ============================================================
// ODREDJIVANJE ROLE:
// [Authorize(Roles = "Admin")] na nivou CELE KLASE znaci da
// SVAKI endpoint u ovom kontroleru zahteva Admin rolu.
// Neprijavljeni korisnici -> 401 Unauthorized
// Prijavljeni koji nisu Admin -> 403 Forbidden
// Samo Admin moze videti, menjati i brisati korisnike.
// ============================================================
// [ROLA] Cela klasa dostupna iskljucivo Adminu
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class KorisniciController : ControllerBase
{
    // Dependancy injection - EF Core kontekst za rad sa bazom podataka
    private readonly ApplicationDbContext _context;

    // Konstruktor prima kontekst kroz DI (konfigurisan u Program.cs)
    public KorisniciController(ApplicationDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------
    // GET /api/korisnici
    // Vraca listu svih korisnika u sistemu (ID, username, rola, datum).
    // Dostupno samo Adminu.
    // Frontend koristi ovo za prikaz admin panela sa svim korisnicima.
    // -------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var korisnici = await _context.Korisnici
            .Select(k => new {
                k.KorisnikId, k.Username, k.Rola, k.DatumRegisdtracije
            }).ToListAsync();
        return Ok(korisnici);
    }

    // -------------------------------------------------------
    // GET /api/korisnici/{id}
    // Vraca detalje jednog korisnika po ID-u.
    // Dostupno samo Adminu.
    // Vraca 404 ako korisnik sa datim ID-em ne postoji.
    // -------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var korisnik = await _context.Korisnici.FindAsync(id);
        if (korisnik == null) return NotFound(new { message = "Korisnik nije pronađen." });
        return Ok(new { korisnik.KorisnikId, korisnik.Username, korisnik.Rola });
    }

    // -------------------------------------------------------
    // PUT /api/korisnici/{id}
    // Menja username i rolu postojeceg korisnika.
    // Dostupno samo Adminu.
    // *** OVDE ADMIN MOZE PROMENITI ROLU DRUGOM KORISNIKU ***
    // Vraca 404 ako korisnik ne postoji, ili 204 No Content pri uspehu.
    // -------------------------------------------------------
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateKorisnikRequest dto)
    {
        var korisnik = await _context.Korisnici.FindAsync(id);
        if (korisnik == null) return NotFound(new { message = "Korisnik nije pronađen." });

        korisnik.Username = dto.Username;
        // [ROLA] Admin moze promeniti rolu bilo kom korisniku
        korisnik.Rola = dto.Rola;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // -------------------------------------------------------
    // DELETE /api/korisnici/{id}
    // Brise korisnika iz baze po ID-u.
    // Dostupno samo Adminu.
    // Vraca 404 ako korisnik ne postoji, ili 204 No Content pri uspehu.
    // -------------------------------------------------------
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var korisnik = await _context.Korisnici.FindAsync(id);
        if (korisnik == null) return NotFound(new { message = "Korisnik nije pronađen." });

        _context.Korisnici.Remove(korisnik);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DTO za izmenu korisnika - frontend salje JSON sa ovim poljima
    public class UpdateKorisnikRequest
    {
        public string Username { get; set; }
        // [ROLA] Nova rola koja ce biti dodeljena korisniku
        public Rola Rola { get; set; }
    }
}
