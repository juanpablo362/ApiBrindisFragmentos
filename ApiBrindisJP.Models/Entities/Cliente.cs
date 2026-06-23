namespace ApiBrindisJP.Models.Entities;

public class Cliente
{
    public string Clave { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public DateOnly FechaNacimiento { get; set; }
    public DateTime FechaRegistro { get; set; }
}
