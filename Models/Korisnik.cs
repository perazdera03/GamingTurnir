namespace GamingTurnir.Models;

// Role korisnika - odredjuju sta korisnik moze da radi
public enum Rola
{
    Admin,    // Puni pristup
    Kapiten,  // Moze da menja timove i dodaje clanove
    Igrac     // Samo pregled
}

// Predstavlja korisnika sistema
public class Korisnik
{
    public int KorisnikId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; } // Lozinka se cuva kao hash, nikad kao tekst
    public Rola Rola { get; set; }
    public DateTime DatumRegisdtracije { get; set; } = DateTime.UtcNow;
}