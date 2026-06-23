using ApiBrindisJP.Data.Security;
using ApiBrindisJP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiBrindisJP.Data.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public AuthRepository(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Usuario?> ValidarCredencialesAsync(string usuario, string password)
    {
        var usuarioDb = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.NombreUsuario == usuario && u.Activo);

        if (usuarioDb is null)
        {
            return null;
        }

        if (_passwordHasher.Verify(password, usuarioDb.PasswordHash))
        {
            return usuarioDb;
        }

        if (usuarioDb.PasswordHash == password)
        {
            usuarioDb.PasswordHash = _passwordHasher.Hash(password);
            await _context.SaveChangesAsync();
            return usuarioDb;
        }

        return null;
    }

    public async Task<bool> ExisteUsuarioAsync(string usuario)
    {
        return await _context.Usuarios.AnyAsync(u => u.NombreUsuario == usuario);
    }

    public async Task<Usuario> RegistrarAsync(string usuario, string password, string? nombreCompleto)
    {
        var nuevoUsuario = new Usuario
        {
            NombreUsuario = usuario,
            PasswordHash = _passwordHasher.Hash(password),
            NombreCompleto = nombreCompleto,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();

        return nuevoUsuario;
    }
}
