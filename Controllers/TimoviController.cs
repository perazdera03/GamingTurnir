using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GamingTurnir.Data;
using GamingTurnir.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TimoviController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TimoviController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var timovi = await _context.Timovi
            .Select(t => new {
                t.TimId, t.Naziv, t.Opis, t.DatumOsnivanja
            }).ToListAsync();
        return Ok(timovi);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tim = await _context.Timovi.FindAsync(id);
        if (tim == null) return NotFound(new { message = "Tim nije pronađen." });
        return Ok(tim);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Tim dto)
    {
        var tim = new Tim
        {
            Naziv = dto.Naziv,
            Opis = dto.Opis,
            DatumOsnivanja = DateTime.SpecifyKind(dto.DatumOsnivanja, DateTimeKind.Utc)
        };
        _context.Timovi.Add(tim);
        await _context.SaveChangesAsync();
        return Created("", new { tim.TimId, tim.Naziv, tim.Opis, tim.DatumOsnivanja });
    }

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
