using ApiBrindisJP.Models.Entities;

namespace ApiBrindisJP.Data.Repositories;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> ObtenerTodosAsync();
    Task<Cliente?> ObtenerPorClaveAsync(string clave);
    Task<bool> ExisteAsync(string clave);
    Task CrearAsync(Cliente cliente);
    Task ActualizarAsync(Cliente cliente);
    Task EliminarAsync(string clave);
}
