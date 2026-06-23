using ApiBrindisJP.Data.Repositories;
using ApiBrindisJP.DTOs.Auth;
using ApiBrindisJP.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiBrindisJP.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IAuthRepository authRepository, IJwtTokenService jwtTokenService)
    {
        _authRepository = authRepository;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Usuario) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Ok(new RegisterResponse
            {
                Success = false,
                Mensaje = "Usuario y contraseña son obligatorios"
            });
        }

        if (request.Password.Length < 6)
        {
            return Ok(new RegisterResponse
            {
                Success = false,
                Mensaje = "La contraseña debe tener al menos 6 caracteres"
            });
        }

        if (await _authRepository.ExisteUsuarioAsync(request.Usuario))
        {
            return Ok(new RegisterResponse
            {
                Success = false,
                Mensaje = "El nombre de usuario ya está registrado"
            });
        }

        var usuario = await _authRepository.RegistrarAsync(
            request.Usuario.Trim(),
            request.Password,
            string.IsNullOrWhiteSpace(request.NombreCompleto) ? null : request.NombreCompleto.Trim());

        return Ok(new RegisterResponse
        {
            Success = true,
            IdUsuario = usuario.IdUsuario,
            Nombre = usuario.NombreCompleto,
            Token = _jwtTokenService.GenerarToken(usuario),
            Mensaje = "Usuario registrado correctamente"
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var usuario = await _authRepository.ValidarCredencialesAsync(request.Usuario, request.Password);

        if (usuario is null)
        {
            return Ok(new LoginResponse
            {
                Success = false,
                Mensaje = "Usuario o contraseña incorrectos"
            });
        }

        return Ok(new LoginResponse
        {
            Success = true,
            IdUsuario = usuario.IdUsuario,
            Nombre = usuario.NombreCompleto,
            Token = _jwtTokenService.GenerarToken(usuario)
        });
    }
}
