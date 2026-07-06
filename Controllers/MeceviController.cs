using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GamingTurnir.Data;
using GamingTurnir.Models;

// ============================================================
// VEZA BACKEND <-> FRONTEND
// ============================================================
// Ruta ovog kontrolera je: /api/mecevi  (definisano u [Route] ispod)
// Frontend JavaScript/Axios poziva ove endpointe, npr:
//   GET    /api/mecevi         -> fetch svih meceva
//   GET    /api/mecevi/{id}    -> fetch jednog meca
//   POST   /api/mecevi         -> kreiranje novog meca (samo Admin)
//   PUT    /api/mecevi/{id}    -> izmena meca (samo Admin)
//   DELETE /api/mecevi/{id}    -> brisanje meca (samo Admin)
//
// Frontend u zahtevu mora da posalje JWT token u headeru:
//   Authorization: Bearer <token>
// Token se dobija pri prijavi (Login endpoint) i cuva se u
// localStorage ili sessionStorage na frontendu.
// ============================================================

// [Authorize] na nivou klase znaci da SVAKI endpoint zahteva
// prijavljenog korisnika (validan JWT token).
// ============================================================
// ODREDJIVANJE ROLE:
// - Gosti (neprijavljeni): nemaju pristup nicemu u ovom kontroleru
// - Prijavljeni korisnici (bilo koja rola): mogu citati meceve (GET)
// - Samo "Admin" rola: moze kreirati, menjati i brisati meceve
//   (oznaceno sa [Authorize(Roles = "Admin")] na tim metodama)
// ============================================================
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MeceviController : ControllerBase
{
    // Dependancy injection - EF Core kontekst za rad sa bazom podataka
    private readonly ApplicationDbContext _context;

    // Konstruktor prima kontekst kroz DI (konfigurisan u Program.cs)
    public MeceviController(ApplicationDbContext context)
    {
        _context = context;
    }

    // DTO (Data Transfer Object) - definise koji podaci stizu sa frontenda
    // pri kreiranju ili izmeni meca (POST i PUT zahtevi).
    // Frontend salje JSON tela koja odgovaraju ovoj klasi.
    public class MecDto
    {
        public int TurnirId { get; set; }
        public int Tim1Id { get; set; }
        public int Tim2Id { get; set; }
        public int? RezultatTim1 { get; set; }
        public int? RezultatTim2 { get; set; }
        public DateTime DatumMeca { get; set; }
    }

    // -------------------------------------------------------
    // GET /api/mecevi
    // Vraca listu svih meceva sa imenima turnira i timova.
    // Dostupno svim prijavljenim korisnicima.
    // Frontend koristi ovo za prikaz tabele svih meceva.
    // -------------------------------------------------------
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

    // -------------------------------------------------------
    // GET /api/mecevi/{id}
    // Vraca detalje jednog meca po ID-u.
    // Dostupno svim prijavljenim korisnicima.
    // Frontend koristi ovo za prikaz detalja konkretnog meca.
    // Vraca 404 ako mec sa datim ID-em ne postoji.
    // -------------------------------------------------------
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

    // -------------------------------------------------------
    // POST /api/mecevi
    // Kreira novi mec u bazi na osnovu podataka iz tela zahteva.
    // [Authorize(Roles = "Admin")] - SAMO Admin moze da kreira mec.
    // *** OVDE SE PROVERAVA ROLA: ako korisnik nije Admin,
    //     server vraca HTTP 403 Forbidden bez izvrsavanja koda. ***
    // Frontend salje JSON koji odgovara MecDto klasi.
    // Vraca 201 Created sa podacima kreiranog meca.
    // -------------------------------------------------------
    // [ROLA] Samo "Admin" ima pristup ovom endpointu
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
            // DateTime se konvertuje u UTC da bi se ispravno cuvao u bazi
            DatumMeca = DateTime.SpecifyKind(dto.DatumMeca, DateTimeKind.Utc)
        };
        _context.Mecevi.Add(mec);
        await _context.SaveChangesAsync();
        return Created("", new { mec.MecId, mec.TurnirId, mec.Tim1Id, mec.Tim2Id, mec.RezultatTim1, mec.RezultatTim2, mec.DatumMeca });
    }

    // -------------------------------------------------------
    // PUT /api/mecevi/{id}
    // Azurira postojeci mec sa novim podacima iz tela zahteva.
    // [Authorize(Roles = "Admin")] - SAMO Admin moze da menja mec.
    // *** OVDE SE PROVERAVA ROLA: ako korisnik nije Admin,
    //     server vraca HTTP 403 Forbidden bez izvrsavanja koda. ***
    // Vraca 404 ako mec ne postoji, ili 204 No Content pri uspehu.
    // -------------------------------------------------------
    // [ROLA] Samo "Admin" ima pristup ovom endpointu
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

    // -------------------------------------------------------
    // DELETE /api/mecevi/{id}
    // Brise mec iz baze po ID-u.
    // [Authorize(Roles = "Admin")] - SAMO Admin moze da brise mec.
    // *** OVDE SE PROVERAVA ROLA: ako korisnik nije Admin,
    //     server vraca HTTP 403 Forbidden bez izvrsavanja koda. ***
    // Vraca 404 ako mec ne postoji, ili 204 No Content pri uspehu.
    // -------------------------------------------------------
    // [ROLA] Samo "Admin" ima pristup ovom endpointu
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
