using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GamingTurnir.Data;
using GamingTurnir.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TurniriController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TurniriController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var turniri = await _context.Turniri
            .Select(t => new {
                t.TurnirId, t.Naziv, t.Igrica, t.DatumPocetka
            }).ToListAsync();
        return Ok(turniri);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var turnir = await _context.Turniri.FindAsync(id);
        if (turnir == null) return NotFound(new { message = "Turnir nije pronađen." });
        return Ok(turnir);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Turnir dto)
    {
        var turnir = new Turnir
        {
            Naziv = dto.Naziv,
            Igrica = dto.Igrica,
            DatumPocetka = DateTime.SpecifyKind(dto.DatumPocetka, DateTimeKind.Utc)
        };
        _context.Turniri.Add(turnir);
        await _context.SaveChangesAsync();
        return Created("", new { turnir.TurnirId, turnir.Naziv, turnir.Igrica, turnir.DatumPocetka });
    }

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