namespace GamingTurnir.Models;

public class Turnir
{
    public int TurnirId { get; set; }
    public string Naziv { get; set; }
    public string Igrica { get; set; }
    public DateTime DatumPocetka { get; set; }

    public ICollection<Mec> Mecevi { get; set; } = new List<Mec>();
}