namespace GamingTurnir.Models;
public enum Rola
{
    Admin, Kapiten, Igrac
}
public class Korisnik
{
    public int KorisnikId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public Rola Rola { get; set; }
    public DateTime DatumRegisdtracije { get; set; } = DateTime.UtcNow;
}