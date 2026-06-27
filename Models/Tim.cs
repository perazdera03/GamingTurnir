namespace GamingTurnir.Models;

public class Tim
{
    public int TimId { get; set; }
    public string Naziv { get; set; }
    public string Opis { get; set; }
    public DateTime DatumOsnivanja { get; set; }

    public ICollection<ClanTima> Clanovi { get; set; } = new List<ClanTima>();
    public ICollection<Mec> MeceviKaoTim1 { get; set; } = new List<Mec>();
    public ICollection<Mec> MeceviKaoTim2 { get; set; } = new List<Mec>();
}