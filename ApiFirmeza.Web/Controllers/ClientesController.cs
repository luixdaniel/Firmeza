using ApiFirmeza.Web.DTOs;
using AutoMapper;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiFirmeza.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly IMapper _mapper;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(
        IClienteService clienteService,
        IMapper mapper,
        ILogger<ClientesController> logger)
    {
        _clienteService = clienteService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los clientes
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
    {
        try
        {
            var clientes = await _clienteService.GetAllAsync();
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
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

            var clienteDto = _mapper.Map<ClienteDto>(cliente);
            return Ok(clienteDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cliente {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Busca clientes por nombre o email
    /// </summary>
    [HttpGet("buscar")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> Search([FromQuery] string criterio)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return Ok(new List<ClienteDto>());

            var clientes = await _clienteService.SearchAsync(criterio);
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
            return Ok(clientesDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar clientes");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo cliente
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> Create([FromBody] ClienteCreateDto clienteDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = _mapper.Map<Cliente>(clienteDto);
            await _clienteService.CreateAsync(cliente);

            var clienteCreado = _mapper.Map<ClienteDto>(cliente);
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
    [Authorize(Roles = "Admin")]
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

            _mapper.Map(clienteDto, clienteExistente);
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
    [Authorize(Roles = "Admin")]
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
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cliente {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene los clientes activos
    /// </summary>
    [HttpGet("activos")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetActivos()
    {
        try
        {
            var clientes = await _clienteService.GetActivosAsync();
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
            return Ok(clientesDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes activos");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}

