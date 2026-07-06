namespace GamingTurnir.Models;

// Enum koji definise moguce role korisnika u sistemu
public enum Rola
{
    Admin,    // Puni pristup - upravljanje svim entitetima
    Kapiten,  // Moze da izmeni tim
    Igrac     // Samo pregled podataka
}

// Predstavlja korisnika sistema.
// Cuva username, hash lozinke, rolu i datum registracije.
// Koristi se za login i autorizaciju putem JWT tokena.
public class Korisnik
{
    // Jedinstveni identifikator korisnika (auto-increment)
    public int KorisnikId { get; set; }
    // Korisnicko ime - mora biti jedinstveno
    public string Username { get; set; }
    // Lozinka se cuva kao hash, nikad kao tekst
    public string PasswordHash { get; set; }
    // Rola odredjuje sta korisnik moze da radi (Admin/Kapiten/Igrac)
    public Rola Rola { get; set; }
    // Automatski se postavlja na trenutno vreme pri registraciji
    public DateTime DatumRegisdtracije { get; set; } = DateTime.UtcNow;
}