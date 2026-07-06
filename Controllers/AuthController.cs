using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamingTurnir.Data;
using GamingTurnir.Models;

// ============================================================
// VEZA BACKEND <-> FRONTEND
// ============================================================
// Ruta ovog kontrolera je: /api/auth
// Frontend poziva ove endpointe pri registraciji i prijavi:
//   POST /api/auth/register  -> registracija novog korisnika
//   POST /api/auth/login     -> prijava i dobijanje JWT tokena
//
// Nakon uspesne prijave, frontend dobija JWT token u odgovoru.
// Token se cuva u localStorage i salje uz svaki naredni zahtev:
//   Authorization: Bearer <token>
//
// NEMA [Authorize] na ovoj klasi - ovi endpointi su javni
// (dostupni svima, bez prijave) jer je to tacka ulaska u sistem.
// ============================================================

// ============================================================
// ODREDJIVANJE ROLE:
// Rola se DODELJUJE pri registraciji (korisnik bira Rola enum).
// Rola se UPISUJE u JWT token kao Claim (ClaimTypes.Role) u
// metodi CreateJwt - od tog trenutka server zna ko je korisnik.
// Dostupne role: Admin, Kapiten, Gledalac (definisano u Rola enum)
// ============================================================
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    // PasswordHasher sluzi za bezbedno cuvanje lozinki (bcrypt-style hash)
    private readonly PasswordHasher<Korisnik> _hasher = new();

    // Konstruktor prima kontekst i konfiguraciju kroz DI
    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // DTO za registraciju - frontend salje JSON sa ovim poljima
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        // [ROLA] Rola se prima od frontenda pri registraciji (Admin, Kapiten, Gledalac)
        public Rola Rola { get; set; }
    }

    // DTO za prijavu - frontend salje samo korisnicko ime i lozinku
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // -------------------------------------------------------
    // POST /api/auth/register
    // Registruje novog korisnika u sistemu.
    // Proverava da li username vec postoji (409 Conflict ako da).
    // Heshuje lozinku pre cuvanja u bazu - nikad ne cuva plain text.
    // Vraca 201 Created sa osnovnim podacima kreiranog korisnika.
    // Javni endpoint - ne zahteva prijavu.
    // -------------------------------------------------------
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        var username = dto.Username.Trim();

        var exists = await _context.Korisnici.AnyAsync(k => k.Username == username);
        if (exists)
            return Conflict(new { message = "Username već postoji." });

        var korisnik = new Korisnik
        {
            Username = username,
            // [ROLA] Rola se direktno preuzima iz zahteva i cuva u bazi
            Rola = dto.Rola
        };

        // Lozinka se heshuje i nikad ne cuva kao plain text
        korisnik.PasswordHash = _hasher.HashPassword(korisnik, dto.Password);

        _context.Korisnici.Add(korisnik);
        await _context.SaveChangesAsync();

        return Created("", new { korisnik.KorisnikId, korisnik.Username, korisnik.Rola });
    }

    // -------------------------------------------------------
    // POST /api/auth/login
    // Proverava kredencijale korisnika i vraca JWT token.
    // Ako korisnik ne postoji ili je lozinka pogresna -> 401 Unauthorized.
    // Ako su kredencijali ispravni -> vraca token koji frontend cuva i
    // salje uz svaki naredni zahtev u Authorization headeru.
    // Javni endpoint - ne zahteva prijavu.
    // -------------------------------------------------------
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var username = dto.Username.Trim();

        var korisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.Username == username);
        if (korisnik == null)
            return Unauthorized(new { message = "Pogrešni kredencijali." });

        // Poredi unetu lozinku sa hashovanom lozinkom iz baze
        var verify = _hasher.VerifyHashedPassword(korisnik, korisnik.PasswordHash, dto.Password);
        if (verify == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Pogrešni kredencijali." });

        var token = CreateJwt(korisnik);

        // Frontend prima token i rolude korisnika - koristi ih za prikaz UI-a
        return Ok(new
        {
            access_token = token,
            token_type = "Bearer",
            username = korisnik.Username,
            // [ROLA] Rola se salje frontendu kako bi znao sta da prikazuje
            rola = korisnik.Rola.ToString()
        });
    }

    // -------------------------------------------------------
    // Privatna pomocna metoda - kreira JWT token za korisnika.
    // Konfiguracija (kljuc, issuer, vreme trajanja) dolazi iz appsettings.json.
    // *** KLJUCNO MESTO ZA ROLU ***
    // Rola se pakuje kao Claim unutar tokena (ClaimTypes.Role).
    // Svaki put kad server primi token, izvlaci Role claim i proverava
    // da li korisnik sme da pristupi endpointu sa [Authorize(Roles=...)].
    // -------------------------------------------------------
    private string CreateJwt(Korisnik korisnik)
    {
        var jwt = _config.GetSection("Jwt");
        var key = jwt["Key"];
        var issuer = jwt["Issuer"];
        var audience = jwt["Audience"];
        var expiryMinutes = int.Parse(jwt["ExpiryMinutes"] ?? "120");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, korisnik.Username),
            new Claim(ClaimTypes.Name, korisnik.Username),
            // [ROLA] Rola se upisuje u token kao Claim - ovo server cita pri svakom zahtevu
            new Claim(ClaimTypes.Role, korisnik.Rola.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}