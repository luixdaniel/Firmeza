using ApiFirmeza.Web.DTOs;
using Firmeza.Web.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiFirmeza.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VentasController : ControllerBase
{
    private readonly IVentaService _ventaService;
    private readonly IProductoService _productoService;
    private readonly ILogger<VentasController> _logger;

    public VentasController(
        IVentaService ventaService, 
        IProductoService productoService,
        ILogger<VentasController> logger)
    {
        _ventaService = ventaService;
        _productoService = productoService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las ventas
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VentaDto>>> GetAll()
    {
        try
        {
            var ventas = await _ventaService.GetAllAsync();
            var ventasDto = ventas.Select(v => new VentaDto
            {
                Id = v.Id,
                FechaVenta = v.FechaVenta,
                NumeroFactura = v.NumeroFactura,
                Cliente = v.Cliente,
                ClienteId = v.ClienteId,
                Total = v.Total,
                Subtotal = v.Subtotal,
                IVA = v.IVA,
                MetodoPago = v.MetodoPago,
                Estado = v.Estado,
                Vendedor = v.Vendedor,
                Detalles = v.Detalles?.Select(d => new DetalleVentaDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto?.Nombre ?? "",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList() ?? new List<DetalleVentaDto>()
            });

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
            var venta = await _ventaService.GetByIdWithDetailsAsync(id);
            if (venta == null)
                return NotFound($"Venta con ID {id} no encontrada");

            var ventaDto = new VentaDto
            {
                Id = venta.Id,
                FechaVenta = venta.FechaVenta,
                NumeroFactura = venta.NumeroFactura,
                Cliente = venta.Cliente,
                ClienteId = venta.ClienteId,
                Total = venta.Total,
                Subtotal = venta.Subtotal,
                IVA = venta.IVA,
                MetodoPago = venta.MetodoPago,
                Estado = venta.Estado,
                Vendedor = venta.Vendedor,
                Detalles = venta.Detalles?.Select(d => new DetalleVentaDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto?.Nombre ?? "",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList() ?? new List<DetalleVentaDto>()
            };

            return Ok(ventaDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener venta {Id}", id);
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

            if (ventaDto.Detalles == null || !ventaDto.Detalles.Any())
                return BadRequest("La venta debe tener al menos un detalle");

            // Verificar stock de productos
            foreach (var detalle in ventaDto.Detalles)
            {
                var producto = await _productoService.GetByIdAsync(detalle.ProductoId);
                if (producto == null)
                    return BadRequest($"Producto con ID {detalle.ProductoId} no encontrado");

                if (producto.Stock < detalle.Cantidad)
                    return BadRequest($"Stock insuficiente para el producto {producto.Nombre}. Stock disponible: {producto.Stock}");
            }

            // Calcular totales
            decimal subtotal = ventaDto.Detalles.Sum(d => d.PrecioUnitario * d.Cantidad);
            decimal iva = subtotal * 0.19m; // 19% IVA
            decimal total = subtotal + iva;

            var venta = new Firmeza.Web.Data.Entities.Venta
            {
                FechaVenta = DateTime.UtcNow,
                NumeroFactura = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Cliente = ventaDto.Cliente,
                ClienteId = ventaDto.ClienteId,
                Subtotal = subtotal,
                IVA = iva,
                Total = total,
                MetodoPago = ventaDto.MetodoPago,
                Estado = "Completada",
                Vendedor = ventaDto.Vendedor,
                Detalles = ventaDto.Detalles.Select(d => new Firmeza.Web.Data.Entities.DetalleDeVenta
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.PrecioUnitario * d.Cantidad
                }).ToList()
            };

            await _ventaService.CrearVentaConDetallesAsync(venta);

            // Actualizar stock de productos
            foreach (var detalle in ventaDto.Detalles)
            {
                var producto = await _productoService.GetByIdAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Stock -= detalle.Cantidad;
                    await _productoService.UpdateAsync(producto);
                }
            }

            var ventaCreada = new VentaDto
            {
                Id = venta.Id,
                FechaVenta = venta.FechaVenta,
                NumeroFactura = venta.NumeroFactura,
                Cliente = venta.Cliente,
                ClienteId = venta.ClienteId,
                Total = venta.Total,
                Subtotal = venta.Subtotal,
                IVA = venta.IVA,
                MetodoPago = venta.MetodoPago,
                Estado = venta.Estado,
                Vendedor = venta.Vendedor,
                Detalles = venta.Detalles.Select(d => new DetalleVentaDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto?.Nombre ?? "",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = venta.Id }, ventaCreada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear venta");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene las ventas de un cliente específico
    /// </summary>
    [HttpGet("cliente/{clienteNombre}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VentaDto>>> GetByCliente(string clienteNombre)
    {
        try
        {
            var ventas = await _ventaService.GetVentasByClienteAsync(clienteNombre);
            var ventasDto = ventas.Select(v => new VentaDto
            {
                Id = v.Id,
                FechaVenta = v.FechaVenta,
                NumeroFactura = v.NumeroFactura,
                Cliente = v.Cliente,
                ClienteId = v.ClienteId,
                Total = v.Total,
                Subtotal = v.Subtotal,
                IVA = v.IVA,
                MetodoPago = v.MetodoPago,
                Estado = v.Estado,
                Vendedor = v.Vendedor,
                Detalles = v.Detalles?.Select(d => new DetalleVentaDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto?.Nombre ?? "",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList() ?? new List<DetalleVentaDto>()
            });

            return Ok(ventasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ventas del cliente {ClienteNombre}", clienteNombre);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}

