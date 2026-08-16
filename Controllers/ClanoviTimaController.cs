using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GamingTurnir.Data;
using GamingTurnir.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClanoviTimaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClanoviTimaController(ApplicationDbContext context)
    {
        _context = context;
    }

    // DTO za kreiranje/izmenu clana tima
    public class ClanTimaDto
    {
        public int KorisnikId { get; set; }
        public int TimId { get; set; }
        public string Uloga { get; set; }
    }

    // GET /api/ClanoviTima - vraca listu svih clanova sa imenima korisnika i timova
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clanovi = await _context.ClanoviTima
            .Include(c => c.Korisnik)
            .Include(c => c.Tim)
            .Select(c => new {
                c.ClanTimaId,
                Korisnik = c.Korisnik.Username,
                Tim = c.Tim.Naziv,
                c.Uloga
            }).ToListAsync();
        return Ok(clanovi);
    }

    // GET /api/ClanoviTima/{id} - vraca jednog clana po ID-u
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var clan = await _context.ClanoviTima
            .Include(c => c.Korisnik)
            .Include(c => c.Tim)
            .FirstOrDefaultAsync(c => c.ClanTimaId == id);
        if (clan == null) return NotFound(new { message = "Clan tima nije pronadjen." });
        return Ok(new {
            clan.ClanTimaId,
            Korisnik = clan.Korisnik.Username,
            Tim = clan.Tim.Naziv,
            clan.Uloga
        });
    }

    // POST /api/ClanoviTima - dodaje novog clana tima (Admin i Kapiten)
    [Authorize(Roles = "Admin,Kapiten")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClanTimaDto dto)
    {
        var clan = new ClanTima
        {
            KorisnikId = dto.KorisnikId,
            TimId = dto.TimId,
            Uloga = dto.Uloga
        };
        _context.ClanoviTima.Add(clan);
        await _context.SaveChangesAsync();
        return Created("", new { clan.ClanTimaId, clan.KorisnikId, clan.TimId, clan.Uloga });
    }

    // PUT /api/ClanoviTima/{id} - menja podatke clana tima (Admin i Kapiten)
    [Authorize(Roles = "Admin,Kapiten")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClanTimaDto dto)
    {
        var clan = await _context.ClanoviTima.FindAsync(id);
        if (clan == null) return NotFound(new { message = "Clan tima nije pronadjen." });

        clan.KorisnikId = dto.KorisnikId;
        clan.TimId = dto.TimId;
        clan.Uloga = dto.Uloga;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/ClanoviTima/{id} - brise clana tima (samo Admin)
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var clan = await _context.ClanoviTima.FindAsync(id);
        if (clan == null) return NotFound(new { message = "Clan tima nije pronadjen." });

        _context.ClanoviTima.Remove(clan);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
