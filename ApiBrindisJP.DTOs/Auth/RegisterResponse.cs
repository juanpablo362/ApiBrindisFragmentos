namespace ApiBrindisJP.DTOs.Auth;

public class RegisterResponse
{
    public bool Success { get; set; }
    public int? IdUsuario { get; set; }
    public string? Nombre { get; set; }
    public string? Token { get; set; }
    public string? Mensaje { get; set; }
}
