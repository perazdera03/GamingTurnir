using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GamingTurnir.Data;
using GamingTurnir.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MeceviController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MeceviController(ApplicationDbContext context)
    {
        _context = context;
    }

    public class MecDto
    {
        public int TurnirId { get; set; }
        public int Tim1Id { get; set; }
        public int Tim2Id { get; set; }
        public int? RezultatTim1 { get; set; }
        public int? RezultatTim2 { get; set; }
        public DateTime DatumMeca { get; set; }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var mecevi = await _context.Mecevi
            .Include(m => m.Turnir)
            .Include(m => m.Tim1)
            .Include(m => m.Tim2)
            .Select(m => new {
                m.MecId,
                m.DatumMeca,
                m.RezultatTim1,
                m.RezultatTim2,
                Turnir = m.Turnir.Naziv,
                Tim1 = m.Tim1.Naziv,
                Tim2 = m.Tim2.Naziv
            }).ToListAsync();
        return Ok(mecevi);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var mec = await _context.Mecevi
            .Include(m => m.Turnir)
            .Include(m => m.Tim1)
            .Include(m => m.Tim2)
            .FirstOrDefaultAsync(m => m.MecId == id);
        if (mec == null) return NotFound(new { message = "Meč nije pronađen." });
        return Ok(new {
            mec.MecId,
            mec.DatumMeca,
            mec.RezultatTim1,
            mec.RezultatTim2,
            Turnir = mec.Turnir.Naziv,
            Tim1 = mec.Tim1.Naziv,
            Tim2 = mec.Tim2.Naziv
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MecDto dto)
    {
        var mec = new Mec
        {
            TurnirId = dto.TurnirId,
            Tim1Id = dto.Tim1Id,
            Tim2Id = dto.Tim2Id,
            RezultatTim1 = dto.RezultatTim1,
            RezultatTim2 = dto.RezultatTim2,
            DatumMeca = DateTime.SpecifyKind(dto.DatumMeca, DateTimeKind.Utc)
        };
        _context.Mecevi.Add(mec);
        await _context.SaveChangesAsync();
        return Created("", new { mec.MecId, mec.TurnirId, mec.Tim1Id, mec.Tim2Id, mec.RezultatTim1, mec.RezultatTim2, mec.DatumMeca });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MecDto dto)
    {
        var mec = await _context.Mecevi.FindAsync(id);
        if (mec == null) return NotFound(new { message = "Meč nije pronađen." });

        mec.TurnirId = dto.TurnirId;
        mec.Tim1Id = dto.Tim1Id;
        mec.Tim2Id = dto.Tim2Id;
        mec.RezultatTim1 = dto.RezultatTim1;
        mec.RezultatTim2 = dto.RezultatTim2;
        mec.DatumMeca = DateTime.SpecifyKind(dto.DatumMeca, DateTimeKind.Utc);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var mec = await _context.Mecevi.FindAsync(id);
        if (mec == null) return NotFound(new { message = "Meč nije pronađen." });

        _context.Mecevi.Remove(mec);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
