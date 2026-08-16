using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GamingTurnir.Data;
using GamingTurnir.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class KorisniciController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public KorisniciController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET /api/Korisnici - vraca listu svih korisnika (dostupno svim prijavljenim)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var korisnici = await _context.Korisnici
            .Select(k => new {
                k.KorisnikId, k.Username, k.Rola, k.DatumRegisdtracije
            }).ToListAsync();
        return Ok(korisnici);
    }

    // GET /api/Korisnici/{id} - vraca jednog korisnika po ID-u
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var korisnik = await _context.Korisnici.FindAsync(id);
        if (korisnik == null) return NotFound(new { message = "Korisnik nije pronađen." });
        return Ok(new { korisnik.KorisnikId, korisnik.Username, korisnik.Rola });
    }

    // PUT /api/Korisnici/{id} - menja username i rolu korisnika (samo Admin)
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateKorisnikRequest dto)
    {
        var korisnik = await _context.Korisnici.FindAsync(id);
        if (korisnik == null) return NotFound(new { message = "Korisnik nije pronađen." });

        korisnik.Username = dto.Username;
        korisnik.Rola = dto.Rola;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/Korisnici/{id} - brise korisnika (samo Admin)
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var korisnik = await _context.Korisnici.FindAsync(id);
        if (korisnik == null) return NotFound(new { message = "Korisnik nije pronađen." });

        _context.Korisnici.Remove(korisnik);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DTO za izmenu korisnika
    public class UpdateKorisnikRequest
    {
        public string Username { get; set; }
        public Rola Rola { get; set; }
    }
}