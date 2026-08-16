namespace GamingTurnir.Models;

// Predstavlja gaming tim u sistemu
public class Tim
{
    public int TimId { get; set; }
    public string Naziv { get; set; }
    public string Opis { get; set; }
    public DateTime DatumOsnivanja { get; set; }

    // Navigaciona svojstva
    public ICollection<ClanTima> Clanovi { get; set; } = new List<ClanTima>();
    public ICollection<Mec> MeceviKaoTim1 { get; set; } = new List<Mec>(); // Mecevi kao prvi tim
    public ICollection<Mec> MeceviKaoTim2 { get; set; } = new List<Mec>(); // Mecevi kao drugi tim
}