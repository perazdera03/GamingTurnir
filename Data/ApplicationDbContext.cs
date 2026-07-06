using Microsoft.EntityFrameworkCore;
using GamingTurnir.Models;

namespace GamingTurnir.Data;

// ============================================================
// ApplicationDbContext je centralna klasa koja predstavlja
// vezu izmedju C# modela i tabela u bazi podataka.
// Entity Framework Core koristi ovu klasu da:
//   - generise SQL upite na osnovu LINQ izraza
//   - prati promene nad objektima (tracking)
//   - primenjuje migracije (struktura baze)
//
// Konfigurisan je u Program.cs i ubacuje se u kontrolere
// kroz Dependency Injection (DI) kao ApplicationDbContext.
// ============================================================
public class ApplicationDbContext : DbContext
{
    // Konstruktor prihvata opcije (connection string, provider)
    // koje se definisu u Program.cs -> builder.Services.AddDbContext(...)
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // ============================================================
    // DbSet-ovi predstavljaju tabele u bazi podataka.
    // Svaki DbSet<T> odgovara jednoj tabeli i koristi se za
    // citanje i pisanje podataka putem LINQ upita.
    // Npr: _context.Korisnici.ToListAsync() -> SELECT * FROM Korisnici
    // ============================================================

    // Tabela korisnika - svaki red je jedan registrovani korisnik
    public DbSet<Korisnik> Korisnici { get; set; }

    // Tabela timova - svaki red je jedan tim
    public DbSet<Tim> Timovi { get; set; }

    // Tabela clanova tima - veza izmedju korisnika i timova (many-to-many)
    public DbSet<ClanTima> ClanoviTima { get; set; }

    // Tabela turnira - svaki red je jedan turnir
    public DbSet<Turnir> Turniri { get; set; }

    // Tabela meceva - svaki red je jedan mec izmedju dva tima
    public DbSet<Mec> Mecevi { get; set; }

    // ============================================================
    // OnModelCreating se poziva jednom pri pokretanju aplikacije.
    // Ovde se definisu relacije i pravila koja EF ne moze
    // automatski da zakljuci iz modela.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relacija: jedan Mec ima jedan Tim1 (kao prvi tim)
        // Tim1 moze biti u vise meceva kao prvi tim (WithMany)
        // Restrict: ne moze se obrisati tim dok postoje mecevi u kojima ucestvuje
        modelBuilder.Entity<Mec>()
            .HasOne(m => m.Tim1)
            .WithMany(t => t.MeceviKaoTim1)
            .HasForeignKey(m => m.Tim1Id)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacija: jedan Mec ima jedan Tim2 (kao drugi tim)
        // Tim2 moze biti u vise meceva kao drugi tim (WithMany)
        // Restrict: isto pravilo kao za Tim1
        modelBuilder.Entity<Mec>()
            .HasOne(m => m.Tim2)
            .WithMany(t => t.MeceviKaoTim2)
            .HasForeignKey(m => m.Tim2Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
