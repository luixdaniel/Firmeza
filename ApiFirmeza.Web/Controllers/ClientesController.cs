using ApiFirmeza.Web.DTOs;
using Firmeza.Web.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiFirmeza.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
    {
        _clienteService = clienteService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los clientes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
    {
        try
        {
            var clientes = await _clienteService.GetAllAsync();
            var clientesDto = clientes.Select(c => new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                NombreCompleto = c.NombreCompleto,
                Email = c.Email,
                Telefono = c.Telefono,
                Documento = c.Documento,
                Direccion = c.Direccion,
                Ciudad = c.Ciudad,
                Pais = c.Pais,
                FechaRegistro = c.FechaRegistro,
                Activo = c.Activo
            });

            return Ok(clientesDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un cliente por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        try
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
                return NotFound($"Cliente con ID {id} no encontrado");

            var clienteDto = new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                NombreCompleto = cliente.NombreCompleto,
                Email = cliente.Email,
                Telefono = cliente.Telefono,
                Documento = cliente.Documento,
                Direccion = cliente.Direccion,
                Ciudad = cliente.Ciudad,
                Pais = cliente.Pais,
                FechaRegistro = cliente.FechaRegistro,
                Activo = cliente.Activo
            };

            return Ok(clienteDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cliente {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo cliente
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> Create([FromBody] ClienteCreateDto clienteDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = new Firmeza.Web.Data.Entities.Cliente
            {
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Email = clienteDto.Email,
                Telefono = clienteDto.Telefono,
                Documento = clienteDto.Documento,
                Direccion = clienteDto.Direccion,
                Ciudad = clienteDto.Ciudad,
                Pais = clienteDto.Pais,
                FechaRegistro = DateTime.UtcNow,
                Activo = true
            };

            await _clienteService.CreateAsync(cliente);

            var clienteCreado = new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                NombreCompleto = cliente.NombreCompleto,
                Email = cliente.Email,
                Telefono = cliente.Telefono,
                Documento = cliente.Documento,
                Direccion = cliente.Direccion,
                Ciudad = cliente.Ciudad,
                Pais = cliente.Pais,
                FechaRegistro = cliente.FechaRegistro,
                Activo = cliente.Activo
            };

            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, clienteCreado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un cliente existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto clienteDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clienteExistente = await _clienteService.GetByIdAsync(id);
            if (clienteExistente == null)
                return NotFound($"Cliente con ID {id} no encontrado");

            clienteExistente.Nombre = clienteDto.Nombre;
            clienteExistente.Apellido = clienteDto.Apellido;
            clienteExistente.Email = clienteDto.Email;
            clienteExistente.Telefono = clienteDto.Telefono;
            clienteExistente.Documento = clienteDto.Documento;
            clienteExistente.Direccion = clienteDto.Direccion;
            clienteExistente.Ciudad = clienteDto.Ciudad;
            clienteExistente.Pais = clienteDto.Pais;
            clienteExistente.Activo = clienteDto.Activo;

            await _clienteService.UpdateAsync(clienteExistente);

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar cliente {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina un cliente
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var resultado = await _clienteService.DeleteAsync(id);
            if (!resultado)
                return NotFound($"Cliente con ID {id} no encontrado");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cliente {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}

