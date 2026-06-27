namespace GamingTurnir.Models;

public class Mec
{
    public int MecId { get; set; }
    public int TurnirId { get; set; }
    public int Tim1Id { get; set; }
    public int Tim2Id { get; set; }
    public int? RezultatTim1 { get; set; }
    public int? RezultatTim2 { get; set; }
    public DateTime DatumMeca { get; set; }

    public Turnir Turnir { get; set; } = null!;
    public Tim Tim1 { get; set; } = null!;
    public Tim Tim2 { get; set; } = null!;
}