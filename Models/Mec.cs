namespace GamingTurnir.Models;

// Predstavlja jedan mec izmedju dva tima u okviru turnira.
// Cuva rezultate oba tima i datum odrzavanja meca.
public class Mec
{
    // Jedinstveni identifikator meca (auto-increment)
    public int MecId { get; set; }
    // Strani kljuc - kom turniru pripada mec
    public int TurnirId { get; set; }
    // Strani kljuc - prvi tim u mecu
    public int Tim1Id { get; set; }
    // Strani kljuc - drugi tim u mecu
    public int Tim2Id { get; set; }
    // Rezultat prvog tima (nullable - moze biti neunesen)
    public int? RezultatTim1 { get; set; }
    // Rezultat drugog tima (nullable - moze biti neunesen)
    public int? RezultatTim2 { get; set; }
    // Datum odrzavanja meca
    public DateTime DatumMeca { get; set; }

    // Navigaciona svojstva - omogucavaju pristup podacima vezanih entiteta
    public Turnir Turnir { get; set; } = null!;
    public Tim Tim1 { get; set; } = null!;
    public Tim Tim2 { get; set; } = null!;
}