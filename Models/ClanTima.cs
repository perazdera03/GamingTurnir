namespace GamingTurnir.Models;

using GamingTurnir.Models;

public class ClanTima
{
    public int ClanTimaId { get; set; }
    public int KorisnikId { get; set; }
    public int TimId { get; set; }
    public string Uloga { get; set; }

    public Korisnik Korisnik { get; set; }
    public Tim Tim { get; set; }
}