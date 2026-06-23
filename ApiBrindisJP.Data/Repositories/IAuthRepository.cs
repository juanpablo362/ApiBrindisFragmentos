using ApiBrindisJP.Models.Entities;

namespace ApiBrindisJP.Data.Repositories;

public interface IAuthRepository
{
    Task<Usuario?> ValidarCredencialesAsync(string usuario, string password);
    Task<bool> ExisteUsuarioAsync(string usuario);
    Task<Usuario> RegistrarAsync(string usuario, string password, string? nombreCompleto);
}
