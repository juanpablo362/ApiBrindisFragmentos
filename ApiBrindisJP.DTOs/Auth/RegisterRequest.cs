namespace ApiBrindisJP.DTOs.Auth;

public class RegisterRequest
{
    public string Usuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? NombreCompleto { get; set; }
}
