namespace GamingTurnir.Models;

// Predstavlja mec izmedju dva tima u okviru turnira
public class Mec
{
    public int MecId { get; set; }
    public int TurnirId { get; set; } // Strani kljuc -> Turniri
    public int Tim1Id { get; set; }   // Strani kljuc -> Timovi (prvi tim)
    public int Tim2Id { get; set; }   // Strani kljuc -> Timovi (drugi tim)
    public int? RezultatTim1 { get; set; } // Nullable - moze biti neunesen
    public int? RezultatTim2 { get; set; } // Nullable - moze biti neunesen
    public DateTime DatumMeca { get; set; }

    // Navigaciona svojstva
    public Turnir Turnir { get; set; } = null!;
    public Tim Tim1 { get; set; } = null!;
    public Tim Tim2 { get; set; } = null!;
}