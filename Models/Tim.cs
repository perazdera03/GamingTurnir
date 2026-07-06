namespace GamingTurnir.Models;

// Predstavlja gaming tim u sistemu.
// Tim moze imati vise clanova i ucestvovati u vise meceva.
public class Tim
{
    // Jedinstveni identifikator tima (auto-increment)
    public int TimId { get; set; }
    // Naziv tima
    public string Naziv { get; set; }
    // Kratak opis tima
    public string Opis { get; set; }
    // Datum kada je tim osnovan
    public DateTime DatumOsnivanja { get; set; }

    // Navigaciono svojstvo - lista clanova tima
    public ICollection<ClanTima> Clanovi { get; set; } = new List<ClanTima>();
    // Navigaciono svojstvo - mecevi u kojima je tim bio Tim1
    public ICollection<Mec> MeceviKaoTim1 { get; set; } = new List<Mec>();
    // Navigaciono svojstvo - mecevi u kojima je tim bio Tim2
    public ICollection<Mec> MeceviKaoTim2 { get; set; } = new List<Mec>();
}