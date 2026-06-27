using Microsoft.EntityFrameworkCore;
using GamingTurnir.Models;

namespace GamingTurnir.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Korisnik> Korisnici { get; set; }
    public DbSet<Tim> Timovi { get; set; }
    public DbSet<ClanTima> ClanoviTima { get; set; }
    public DbSet<Turnir> Turniri { get; set; }
    public DbSet<Mec> Mecevi { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mec>()
            .HasOne(m => m.Tim1)
            .WithMany(t => t.MeceviKaoTim1)
            .HasForeignKey(m => m.Tim1Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mec>()
            .HasOne(m => m.Tim2)
            .WithMany(t => t.MeceviKaoTim2)
            .HasForeignKey(m => m.Tim2Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}