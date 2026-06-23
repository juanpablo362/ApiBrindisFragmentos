namespace ApiBrindisJP.DTOs.Clientes;

public class ClienteUpdateRequest
{
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public DateOnly FechaNacimiento { get; set; }
}
