namespace GamingTurnir.Models;

using GamingTurnir.Models;

// Veza izmedju korisnika i tima (many-to-many)
public class ClanTima
{
    public int ClanTimaId { get; set; }
    public int KorisnikId { get; set; } // Strani kljuc -> Korisnici
    public int TimId { get; set; }      // Strani kljuc -> Timovi
    public string Uloga { get; set; }   // Uloga u timu (npr. Kapiten, Igrac)

    // Navigaciona svojstva
    public Korisnik Korisnik { get; set; }
    public Tim Tim { get; set; }
}