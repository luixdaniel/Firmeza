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
public class VentasController : ControllerBase
{
    private readonly IVentaService _ventaService;
    private readonly IClienteService _clienteService;
    private readonly IProductoService _productoService;
    private readonly IMapper _mapper;
    private readonly ILogger<VentasController> _logger;

    public VentasController(
        IVentaService ventaService,
        IClienteService clienteService,
        IProductoService productoService,
        IMapper mapper,
        ILogger<VentasController> logger)
    {
        _ventaService = ventaService;
        _clienteService = clienteService;
        _productoService = productoService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las ventas
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VentaDto>>> GetAll()
    {
        try
        {
            var ventas = await _ventaService.GetAllAsync();
            var ventasDto = _mapper.Map<IEnumerable<VentaDto>>(ventas);
            return Ok(ventasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ventas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una venta por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VentaDto>> GetById(int id)
    {
        try
        {
            var venta = await _ventaService.GetByIdAsync(id);
            if (venta == null)
                return NotFound($"Venta con ID {id} no encontrada");

            var ventaDto = _mapper.Map<VentaDto>(venta);
            return Ok(ventaDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener venta {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene ventas por cliente
    /// </summary>
    [HttpGet("cliente/{clienteId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VentaDto>>> GetByCliente(int clienteId)
    {
        try
        {
            var ventas = await _ventaService.GetByClienteIdAsync(clienteId);
            var ventasDto = _mapper.Map<IEnumerable<VentaDto>>(ventas);
            return Ok(ventasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ventas del cliente {ClienteId}", clienteId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene las ventas del cliente autenticado actual
    /// </summary>
    [HttpGet("mis-compras")]
    [Authorize(Roles = "Cliente,Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VentaDto>>> GetMisCompras()
    {
        try
        {
            // Obtener el email del usuario autenticado
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return NotFound("No se pudo obtener el email del usuario autenticado");

            // Buscar el cliente por email
            var clientes = await _clienteService.GetAllAsync();
            var cliente = clientes.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            
            if (cliente == null)
                return Ok(new List<VentaDto>()); // Retornar lista vacía si no hay cliente

            // Obtener las ventas del cliente
            var ventas = await _ventaService.GetByClienteIdAsync(cliente.Id);
            var ventasDto = _mapper.Map<IEnumerable<VentaDto>>(ventas);
            return Ok(ventasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener compras del cliente");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene ventas por rango de fechas
    /// </summary>
    [HttpGet("fecha-rango")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VentaDto>>> GetByFechaRango(
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin)
    {
        try
        {
            var ventas = await _ventaService.GetByFechaRangoAsync(fechaInicio, fechaFin);
            var ventasDto = _mapper.Map<IEnumerable<VentaDto>>(ventas);
            return Ok(ventasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ventas por rango de fechas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea una nueva venta
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VentaDto>> Create([FromBody] VentaCreateDto ventaDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Obtener el cliente del usuario autenticado
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return BadRequest("No se pudo obtener el email del usuario autenticado");

            var clientes = await _clienteService.GetAllAsync();
            var cliente = clientes.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            
            if (cliente == null)
                return BadRequest("Cliente no encontrado. Por favor, complete su perfil primero.");

            // Si el DTO no tiene ClienteId, usar el del usuario autenticado
            if (ventaDto.ClienteId == 0)
            {
                ventaDto.ClienteId = cliente.Id;
            }
            // Si tiene ClienteId, verificar que sea válido
            else if (ventaDto.ClienteId != cliente.Id)
            {
                // Solo Admin puede crear ventas para otros clientes
                if (!User.IsInRole("Admin"))
                    return BadRequest("No tienes permiso para crear ventas para otros clientes");
                    
                cliente = await _clienteService.GetByIdAsync(ventaDto.ClienteId);
                if (cliente == null)
                    return BadRequest($"Cliente con ID {ventaDto.ClienteId} no encontrado");
            }

            // Mapear DTO a entidad
            var venta = _mapper.Map<Venta>(ventaDto);
            venta.Cliente = $"{cliente.Nombre} {cliente.Apellido}";
            venta.ClienteId = cliente.Id;
            venta.MetodoPago = string.IsNullOrEmpty(ventaDto.MetodoPago) ? "Efectivo" : ventaDto.MetodoPago;
            
            // Validar stock de productos antes de crear la venta
            foreach (var detalle in venta.Detalles)
            {
                var producto = await _productoService.GetByIdAsync(detalle.ProductoId);
                if (producto == null)
                    return BadRequest($"Producto con ID {detalle.ProductoId} no encontrado");

                if (producto.Stock < detalle.Cantidad)
                    return BadRequest($"Stock insuficiente para el producto '{producto.Nombre}'. Stock disponible: {producto.Stock}");
            }

            // Usar CrearVentaConDetallesAsync que maneja todo el proceso
            await _ventaService.CrearVentaConDetallesAsync(venta);

            var ventaCreada = _mapper.Map<VentaDto>(venta);
            return CreatedAtAction(nameof(GetById), new { id = venta.Id }, ventaCreada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear venta");
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza una venta existente
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] VentaUpdateDto ventaDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ventaExistente = await _ventaService.GetByIdAsync(id);
            if (ventaExistente == null)
                return NotFound($"Venta con ID {id} no encontrada");

            // Validar cliente
            var cliente = await _clienteService.GetByIdAsync(ventaDto.ClienteId);
            if (cliente == null)
                return BadRequest($"Cliente con ID {ventaDto.ClienteId} no encontrado");

            _mapper.Map(ventaDto, ventaExistente);
            ventaExistente.Cliente = $"{cliente.Nombre} {cliente.Apellido}";
            
            // Recalcular totales
            decimal total = 0;
            foreach (var detalle in ventaExistente.Detalles)
            {
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                total += detalle.Subtotal;
            }
            
            ventaExistente.Subtotal = total;
            ventaExistente.IVA = total * 0.19m;
            ventaExistente.Total = total + ventaExistente.IVA;

            await _ventaService.UpdateAsync(ventaExistente);

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar venta {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina una venta (solo Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var resultado = await _ventaService.DeleteAsync(id);
            if (!resultado)
                return NotFound($"Venta con ID {id} no encontrada");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar venta {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene el total de ventas en un período
    /// </summary>
    [HttpGet("total-periodo")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetTotalPeriodo(
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin)
    {
        try
        {
            var total = await _ventaService.GetTotalVentasPeriodoAsync(fechaInicio, fechaFin);
            return Ok(new { fechaInicio, fechaFin, total });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al calcular total de ventas");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}

