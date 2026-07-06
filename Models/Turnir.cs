namespace GamingTurnir.Models;

// Predstavlja gaming turnir.
// Turnir ima naziv, naziv igrice i datum pocetka.
// Jedan turnir moze imati vise meceva.
public class Turnir
{
    // Jedinstveni identifikator turnira (auto-increment)
    public int TurnirId { get; set; }
    // Naziv turnira (npr. "CS2 Open 2024")
    public string Naziv { get; set; }
    // Naziv igrice za koju se organizuje turnir
    public string Igrica { get; set; }
    // Datum pocetka turnira
    public DateTime DatumPocetka { get; set; }

    // Navigaciono svojstvo - lista meceva u ovom turniru
    public ICollection<Mec> Mecevi { get; set; } = new List<Mec>();
}