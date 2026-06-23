namespace ApiBrindisJP.DTOs.Clientes;

public class ClienteCreateRequest
{
    public string Clave { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public DateOnly FechaNacimiento { get; set; }
}
