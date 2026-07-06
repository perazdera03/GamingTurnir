namespace GamingTurnir.Models;

using GamingTurnir.Models;

// Predstavlja vezu izmedju korisnika i tima (many-to-many).
// Cuva ko je clan kog tima i koja mu je uloga u timu.
public class ClanTima
{
    // Jedinstveni identifikator clana tima (auto-increment)
    public int ClanTimaId { get; set; }
    // Strani kljuc - koji korisnik je clan
    public int KorisnikId { get; set; }
    // Strani kljuc - kojeg tima je clan
    public int TimId { get; set; }
    // Uloga clana u timu (npr. Kapiten, Igrac)
    public string Uloga { get; set; }

    // Navigaciono svojstvo - korisnik koji je clan
    public Korisnik Korisnik { get; set; }
    // Navigaciono svojstvo - tim u kojem je clan
    public Tim Tim { get; set; }
}