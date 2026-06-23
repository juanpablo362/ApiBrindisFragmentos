using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBrindisJP.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/debug")]
public class DebugController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public DebugController(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            status = "ok",
            message = "Endpoint de prueba sin base de datos",
            service = "ApiBrindisJP",
            utc = DateTime.UtcNow
        });
    }

    [HttpGet("info")]
    public IActionResult Info()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        return Ok(new
        {
            status = "ok",
            environment = _environment.EnvironmentName,
            utc = DateTime.UtcNow,
            hasConnectionString = !string.IsNullOrWhiteSpace(connectionString),
            hasJwtKey = !string.IsNullOrWhiteSpace(_configuration["Jwt:Key"]),
            version = typeof(Program).Assembly.GetName().Version?.ToString()
        });
    }
}
