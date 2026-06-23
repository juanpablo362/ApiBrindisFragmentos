using ApiBrindisJP.Data.Repositories;
using ApiBrindisJP.DTOs.Clientes;
using ApiBrindisJP.DTOs.Common;
using ApiBrindisJP.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBrindisJP.Controllers;

[ApiController]
[Authorize]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly IClienteRepository _clienteRepository;

    public ClientesController(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteResponse>>> GetClientes()
    {
        var clientes = await _clienteRepository.ObtenerTodosAsync();
        return Ok(clientes.Select(MapToResponse));
    }

    [HttpGet("{clave}")]
    public async Task<ActionResult<ClienteResponse>> GetCliente(string clave)
    {
        var cliente = await _clienteRepository.ObtenerPorClaveAsync(clave);

        if (cliente is null)
        {
            return NotFound(new MensajeResponse { Mensaje = "Cliente no encontrado" });
        }

        return Ok(MapToResponse(cliente));
    }

    [HttpPost]
    public async Task<ActionResult<MensajeResponse>> CrearCliente([FromBody] ClienteCreateRequest request)
    {
        if (await _clienteRepository.ExisteAsync(request.Clave))
        {
            return Conflict(new MensajeResponse { Mensaje = "La clave del cliente ya existe" });
        }

        var cliente = new Cliente
        {
            Clave = request.Clave,
            Nombre = request.Nombre,
            Edad = request.Edad,
            FechaNacimiento = request.FechaNacimiento
        };

        await _clienteRepository.CrearAsync(cliente);

        return Ok(new MensajeResponse { Mensaje = "Cliente guardado correctamente" });
    }

    [HttpPut("{clave}")]
    public async Task<ActionResult<MensajeResponse>> ActualizarCliente(string clave, [FromBody] ClienteUpdateRequest request)
    {
        var cliente = await _clienteRepository.ObtenerPorClaveAsync(clave);

        if (cliente is null)
        {
            return NotFound(new MensajeResponse { Mensaje = "Cliente no encontrado" });
        }

        cliente.Nombre = request.Nombre;
        cliente.Edad = request.Edad;
        cliente.FechaNacimiento = request.FechaNacimiento;

        await _clienteRepository.ActualizarAsync(cliente);

        return Ok(new MensajeResponse { Mensaje = "Cliente actualizado correctamente" });
    }

    [HttpDelete("{clave}")]
    public async Task<ActionResult<MensajeResponse>> EliminarCliente(string clave)
    {
        if (!await _clienteRepository.ExisteAsync(clave))
        {
            return NotFound(new MensajeResponse { Mensaje = "Cliente no encontrado" });
        }

        await _clienteRepository.EliminarAsync(clave);

        return Ok(new MensajeResponse { Mensaje = "Cliente eliminado correctamente" });
    }

    private static ClienteResponse MapToResponse(Cliente cliente)
    {
        return new ClienteResponse
        {
            Clave = cliente.Clave,
            Nombre = cliente.Nombre,
            Edad = cliente.Edad,
            FechaNacimiento = cliente.FechaNacimiento
        };
    }
}
