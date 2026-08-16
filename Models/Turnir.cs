namespace GamingTurnir.Models;

// Predstavlja gaming turnir
public class Turnir
{
    public int TurnirId { get; set; }
    public string Naziv { get; set; }
    public string Igrica { get; set; }
    public DateTime DatumPocetka { get; set; }

    // Navigaciono svojstvo - jedan turnir moze imati vise meceva
    public ICollection<Mec> Mecevi { get; set; } = new List<Mec>();
}