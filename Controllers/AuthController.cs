using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamingTurnir.Data;
using GamingTurnir.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly PasswordHasher<Korisnik> _hasher = new();

    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Rola Rola { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

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
            Rola = dto.Rola
        };

        korisnik.PasswordHash = _hasher.HashPassword(korisnik, dto.Password);

        _context.Korisnici.Add(korisnik);
        await _context.SaveChangesAsync();

        return Created("", new { korisnik.KorisnikId, korisnik.Username, korisnik.Rola });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var username = dto.Username.Trim();

        var korisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.Username == username);
        if (korisnik == null)
            return Unauthorized(new { message = "Pogrešni kredencijali." });

        var verify = _hasher.VerifyHashedPassword(korisnik, korisnik.PasswordHash, dto.Password);
        if (verify == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Pogrešni kredencijali." });

        var token = CreateJwt(korisnik);

        return Ok(new
        {
            access_token = token,
            token_type = "Bearer",
            username = korisnik.Username,
            rola = korisnik.Rola.ToString()
        });
    }

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