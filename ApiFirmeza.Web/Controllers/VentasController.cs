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
    private readonly ApiFirmeza.Web.Services.IEmailService _emailService;
    private readonly ApiFirmeza.Web.Services.IComprobanteService _comprobanteService;
    
    private const decimal IVA_PORCENTAJE = 0.16m; // 16% de IVA

    public VentasController(
        IVentaService ventaService,
        IClienteService clienteService,
        IProductoService productoService,
        IMapper mapper,
        ILogger<VentasController> logger,
        ApiFirmeza.Web.Services.IEmailService emailService,
        ApiFirmeza.Web.Services.IComprobanteService comprobanteService)
    {
        _ventaService = ventaService;
        _clienteService = clienteService;
        _productoService = productoService;
        _mapper = mapper;
        _logger = logger;
        _emailService = emailService;
        _comprobanteService = comprobanteService;
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
            // Logging para diagnóstico
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            _logger.LogInformation("🔍 GetMisCompras - Email del token: {Email}", email);
            
            var cliente = await ObtenerClienteAutenticadoAsync();
            if (cliente == null)
            {
                _logger.LogWarning("⚠️ GetMisCompras - No se encontró cliente para el email: {Email}", email);
                return Ok(new List<VentaDto>()); // Retornar lista vacía si no hay cliente
            }

            _logger.LogInformation("✅ GetMisCompras - Cliente encontrado: ID={ClienteId}, Nombre={NombreCompleto}", 
                cliente.Id, cliente.NombreCompleto);

            // Obtener las ventas del cliente
            var ventas = await _ventaService.GetByClienteIdAsync(cliente.Id);
            _logger.LogInformation("📊 GetMisCompras - Ventas encontradas: {Count}", ventas.Count());
            
            var ventasDto = _mapper.Map<IEnumerable<VentaDto>>(ventas);
            return Ok(ventasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener compras del cliente");
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
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

            _logger.LogInformation("🛒 Creando venta - Método de pago: {MetodoPago}, Detalles: {Count}", 
                ventaDto.MetodoPago, ventaDto.Detalles?.Count ?? 0);

            // Obtener el cliente del usuario autenticado
            var cliente = await ObtenerClienteAutenticadoAsync();
            if (cliente == null)
            {
                _logger.LogError("❌ Create Venta - Cliente no encontrado para el usuario autenticado");
                return BadRequest(new { message = "Cliente no encontrado. Por favor, complete su perfil primero." });
            }

            _logger.LogInformation("✅ Create Venta - Cliente autenticado: ID={ClienteId}, Nombre={Nombre}", 
                cliente.Id, cliente.NombreCompleto);

            // Si el DTO no tiene ClienteId, usar el del usuario autenticado
            if (ventaDto.ClienteId == 0)
            {
                ventaDto.ClienteId = cliente.Id;
                _logger.LogInformation("📝 Create Venta - ClienteId asignado desde usuario autenticado: {ClienteId}", cliente.Id);
            }
            // Si tiene ClienteId, verificar que sea válido
            else if (ventaDto.ClienteId != cliente.Id)
            {
                // Solo Admin puede crear ventas para otros clientes
                if (!User.IsInRole("Admin"))
                    return BadRequest(new { message = "No tienes permiso para crear ventas para otros clientes" });
                    
                cliente = await _clienteService.GetByIdAsync(ventaDto.ClienteId);
                if (cliente == null)
                    return BadRequest(new { message = $"Cliente con ID {ventaDto.ClienteId} no encontrado" });
            }

            // Mapear DTO a entidad
            var venta = _mapper.Map<Venta>(ventaDto);
            venta.Cliente = $"{cliente.Nombre} {cliente.Apellido}";
            venta.ClienteId = cliente.Id;
            venta.MetodoPago = string.IsNullOrEmpty(ventaDto.MetodoPago) ? "Efectivo" : ventaDto.MetodoPago;
            
            _logger.LogInformation("📦 Create Venta - Venta mapeada: ClienteId={ClienteId}, Cliente={Cliente}, MetodoPago={MetodoPago}", 
                venta.ClienteId, venta.Cliente, venta.MetodoPago);
            
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

            _logger.LogInformation("✅ Create Venta - Venta creada exitosamente: VentaId={VentaId}, ClienteId={ClienteId}, Total={Total}", 
                venta.Id, venta.ClienteId, venta.Total);

            // Enviar comprobante por email (no bloqueante)
            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation("📧 Iniciando envío de comprobante por email para Venta ID: {VentaId}", venta.Id);
                    
                    // Obtener venta completa con detalles para el PDF
                    var ventaCompleta = await _ventaService.GetByIdAsync(venta.Id);
                    if (ventaCompleta == null)
                    {
                        _logger.LogWarning("⚠️ No se pudo obtener la venta completa para enviar email");
                        return;
                    }

                    // Generar PDF del comprobante
                    var pdfBytes = _comprobanteService.GenerarComprobantePdf(ventaCompleta);
                    
                    // Enviar email con el comprobante
                    var emailEnviado = await _emailService.EnviarComprobanteCompraAsync(
                        destinatario: cliente.Email,
                        nombreCliente: cliente.NombreCompleto,
                        ventaId: venta.Id,
                        total: venta.Total,
                        numeroFactura: venta.NumeroFactura,
                        pdfBytes: pdfBytes
                    );

                    if (emailEnviado)
                    {
                        _logger.LogInformation("✅ Comprobante enviado exitosamente a {Email}", cliente.Email);
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ No se pudo enviar el comprobante a {Email}", cliente.Email);
                    }
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "❌ Error al enviar comprobante por email");
                }
            });

            var ventaCreada = _mapper.Map<VentaDto>(venta);
            
            // Agregar información de que el comprobante será enviado
            return CreatedAtAction(nameof(GetById), new { id = venta.Id }, new 
            { 
                venta = ventaCreada,
                mensaje = "Compra realizada exitosamente. El comprobante será enviado a tu correo electrónico."
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "❌ Error de validación al crear venta: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al crear venta");
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
            
            // Recalcular totales con el porcentaje de IVA correcto
            decimal total = 0;
            foreach (var detalle in ventaExistente.Detalles)
            {
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                total += detalle.Subtotal;
            }
            
            ventaExistente.Subtotal = total;
            ventaExistente.IVA = total * IVA_PORCENTAJE; // Usando constante
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

    #region Métodos Privados Auxiliares

    /// <summary>
    /// Obtiene el cliente asociado al usuario autenticado actual
    /// </summary>
    private async Task<Cliente?> ObtenerClienteAutenticadoAsync()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            _logger.LogWarning("⚠️ ObtenerClienteAutenticadoAsync - No se encontró email en el token");
            return null;
        }

        _logger.LogInformation("🔍 ObtenerClienteAutenticadoAsync - Buscando cliente con email: {Email}", email);
        
        // Usar GetByEmailAsync para búsqueda eficiente en base de datos
        var cliente = await _clienteService.GetByEmailAsync(email);
        
        if (cliente == null)
        {
            _logger.LogWarning("⚠️ ObtenerClienteAutenticadoAsync - Cliente no encontrado en BD para email: {Email}", email);
        }
        else
        {
            _logger.LogInformation("✅ ObtenerClienteAutenticadoAsync - Cliente encontrado: ID={Id}, Nombre={Nombre}", 
                cliente.Id, cliente.NombreCompleto);
        }
        
        return cliente;
    }

    #endregion
}