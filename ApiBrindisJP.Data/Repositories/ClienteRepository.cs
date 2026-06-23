using ApiBrindisJP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiBrindisJP.Data.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
    {
        return await _context.Clientes
            .AsNoTracking()
            .OrderBy(c => c.Clave)
            .ToListAsync();
    }

    public async Task<Cliente?> ObtenerPorClaveAsync(string clave)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Clave == clave);
    }

    public async Task<bool> ExisteAsync(string clave)
    {
        return await _context.Clientes.AnyAsync(c => c.Clave == clave);
    }

    public async Task CrearAsync(Cliente cliente)
    {
        cliente.FechaRegistro = DateTime.UtcNow;
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Cliente cliente)
    {
        var existente = await _context.Clientes.FindAsync(cliente.Clave);
        if (existente is null)
        {
            return;
        }

        existente.Nombre = cliente.Nombre;
        existente.Edad = cliente.Edad;
        existente.FechaNacimiento = cliente.FechaNacimiento;
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(string clave)
    {
        var cliente = await _context.Clientes.FindAsync(clave);
        if (cliente is not null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
