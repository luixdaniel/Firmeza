using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;

namespace Firmeza.Web.Services;

public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPdfService? _pdfService;

    public VentaService(IVentaRepository ventaRepository, IProductoRepository productoRepository, IClienteRepository clienteRepository, IPdfService? pdfService = null)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
        _clienteRepository = clienteRepository;
        _pdfService = pdfService;
    }

    public async Task<IEnumerable<Venta>> GetAllAsync()
    {
        try
        {
            return await _ventaRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener todas las ventas: {ex.Message}", ex);
        }
    }

    public async Task<Venta?> GetByIdAsync(int id)
    {
        try
        {
            return await _ventaRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la venta con ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<Venta?> GetByIdWithDetailsAsync(int id)
    {
        try
        {
            return await _ventaRepository.GetByIdWithDetailsAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la venta con detalles: {ex.Message}", ex);
        }
    }

    public async Task<Venta> CreateAsync(Venta venta)
    {
        try
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(venta.Cliente))
                throw new ArgumentException("El cliente es requerido.");

            if (string.IsNullOrWhiteSpace(venta.MetodoPago))
                throw new ArgumentException("El método de pago es requerido.");

            if (venta.Total <= 0)
                throw new ArgumentException("El total debe ser mayor a 0.");

            // Generar número de factura si no existe
            if (string.IsNullOrWhiteSpace(venta.NumeroFactura))
            {
                venta.NumeroFactura = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            }

            // Verificar que no exista el número de factura
            var exists = await _ventaRepository.ExistsByNumeroFacturaAsync(venta.NumeroFactura);
            if (exists)
                throw new ArgumentException($"Ya existe una venta con el número de factura '{venta.NumeroFactura}'.");

            venta.FechaVenta = DateTime.UtcNow;
            venta.Estado = "Completada";

            await _ventaRepository.AddAsync(venta);
            return venta;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear la venta: {ex.Message}", ex);
        }
    }

    public async Task<Venta> CrearVentaConDetallesAsync(Venta venta)
    {
        Console.WriteLine($"=== VentaService.CrearVentaConDetallesAsync ===");
        Console.WriteLine($"Cliente: {venta.Cliente}");
        Console.WriteLine($"Detalles count: {venta.Detalles?.Count ?? 0}");
        
        try
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(venta.Cliente))
                throw new ArgumentException("El cliente es requerido.");

            if (venta.Detalles == null || !venta.Detalles.Any())
                throw new ArgumentException("La venta debe tener al menos un detalle.");

            Console.WriteLine("Validaciones pasadas, validando stock...");

            // Validar stock de productos
            foreach (var detalle in venta.Detalles)
            {
                Console.WriteLine($"Validando producto {detalle.ProductoId}...");
                var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                if (producto == null)
                    throw new ArgumentException($"El producto con ID {detalle.ProductoId} no existe.");

                if (producto.Stock < detalle.Cantidad)
                    throw new InvalidOperationException($"Stock insuficiente para el producto '{producto.Nombre}'. Stock disponible: {producto.Stock}");

                // Establecer precio unitario del producto actual
                detalle.PrecioUnitario = producto.Precio;
                detalle.CalcularSubtotal();
                Console.WriteLine($"Producto {producto.Nombre} validado. Subtotal: {detalle.Subtotal}");
            }

            // Calcular totales
            venta.Subtotal = venta.Detalles.Sum(d => d.Subtotal);
            venta.IVA = venta.Subtotal * 0.16m; // 16% IVA (ajustar según tu país)
            venta.Total = venta.Subtotal + venta.IVA;

            Console.WriteLine($"Subtotal: {venta.Subtotal}, IVA: {venta.IVA}, Total: {venta.Total}");

            // El ClienteId ya debe venir establecido desde el controlador
            if (!venta.ClienteId.HasValue || venta.ClienteId.Value <= 0)
            {
                throw new ArgumentException("El ClienteId es requerido para crear una venta.");
            }
            
            Console.WriteLine($"Cliente ID: {venta.ClienteId}, Nombre: {venta.Cliente}");

            // Generar número de factura
            venta.NumeroFactura = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            venta.FechaVenta = DateTime.UtcNow;
            venta.Estado = "Completada";
            
            // Establecer vendedor por defecto si no está establecido
            if (string.IsNullOrEmpty(venta.Vendedor))
            {
                venta.Vendedor = "Sistema";
            }

            Console.WriteLine($"Numero factura: {venta.NumeroFactura}, Vendedor: {venta.Vendedor}");
            Console.WriteLine("Configurando relaciones de detalles con la venta...");
            
            // Asegurar que los detalles estén correctamente vinculados a la venta
            // Esto evita problemas de relación en EF Core
            if (venta.Detalles != null && venta.Detalles.Any())
            {
                foreach (var detalle in venta.Detalles)
                {
                    detalle.Venta = venta;
                    detalle.VentaId = 0; // Se establecerá automáticamente por EF
                    Console.WriteLine($"Detalle configurado: ProductoId={detalle.ProductoId}, Cantidad={detalle.Cantidad}");
                }
            }
            
            Console.WriteLine("Guardando venta en la base de datos...");

            // Guardar venta
            await _ventaRepository.AddAsync(venta);

            Console.WriteLine($"Venta guardada con ID: {venta.Id}");
            Console.WriteLine("Actualizando stock de productos...");

            // Actualizar stock de productos
            foreach (var detalle in venta.Detalles)
            {
                var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                if (producto != null)
                {
                    Console.WriteLine($"Actualizando stock de {producto.Nombre}: {producto.Stock} -> {producto.Stock - detalle.Cantidad}");
                    producto.Stock -= detalle.Cantidad;
                    await _productoRepository.UpdateAsync(producto);
                }
            }

            Console.WriteLine("Venta creada exitosamente!");
            
            // Generar recibo PDF automáticamente (solo si el servicio está disponible)
            if (_pdfService != null)
            {
                try
                {
                    Console.WriteLine("Generando recibo PDF...");
                    var rutaPdf = await _pdfService.GenerarReciboPdfAsync(venta);
                    Console.WriteLine($"Recibo PDF generado: {rutaPdf}");
                }
                catch (Exception pdfEx)
                {
                    Console.WriteLine($"Error al generar recibo PDF: {pdfEx.Message}");
                    // No lanzamos excepción para no afectar la creación de la venta
                }
            }
            
            return venta;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR en CrearVentaConDetallesAsync: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw new Exception($"Error al crear la venta con detalles: {ex.Message}", ex);
        }
    }

    public async Task<Venta> UpdateAsync(Venta venta)
    {
        try
        {
            var exists = await _ventaRepository.ExistsAsync(venta.Id);
            if (!exists)
                throw new ArgumentException("La venta no existe.");

            await _ventaRepository.UpdateAsync(venta);
            return venta;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar la venta: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var exists = await _ventaRepository.ExistsAsync(id);
            if (!exists)
                return false;

            // Obtener la venta con detalles para restaurar stock
            var venta = await _ventaRepository.GetByIdWithDetailsAsync(id);
            if (venta != null && venta.Estado == "Completada")
            {
                // Restaurar stock de productos
                foreach (var detalle in venta.Detalles)
                {
                    var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock += detalle.Cantidad;
                        await _productoRepository.UpdateAsync(producto);
                    }
                }
            }

            await _ventaRepository.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar la venta: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            return await _ventaRepository.ExistsAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar la existencia de la venta: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsByNumeroFacturaAsync(string numeroFactura, int? excludeId = null)
    {
        try
        {
            return await _ventaRepository.ExistsByNumeroFacturaAsync(numeroFactura, excludeId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar el número de factura: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Venta>> GetVentasByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        try
        {
            return await _ventaRepository.GetVentasByFechaRangoAsync(fechaInicio, fechaFin);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener ventas por rango de fechas: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Venta>> GetVentasByEstadoAsync(string estado)
    {
        try
        {
            return await _ventaRepository.GetVentasByEstadoAsync(estado);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener ventas por estado: {ex.Message}", ex);
        }
    }

    public async Task<decimal> GetTotalVentasByFechaAsync(DateTime fecha)
    {
        try
        {
            return await _ventaRepository.GetTotalVentasByFechaAsync(fecha);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el total de ventas por fecha: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Venta>> GetVentasByClienteAsync(string cliente)
    {
        try
        {
            return await _ventaRepository.GetVentasByClienteAsync(cliente);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener ventas por cliente: {ex.Message}", ex);
        }
    }

    public async Task<bool> CancelarVentaAsync(int id)
    {
        try
        {
            var venta = await _ventaRepository.GetByIdWithDetailsAsync(id);
            if (venta == null)
                return false;

            if (venta.Estado == "Cancelada")
                throw new InvalidOperationException("La venta ya está cancelada.");

            // Restaurar stock si la venta estaba completada
            if (venta.Estado == "Completada")
            {
                foreach (var detalle in venta.Detalles)
                {
                    var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock += detalle.Cantidad;
                        await _productoRepository.UpdateAsync(producto);
                    }
                }
            }

            venta.Estado = "Cancelada";
            await _ventaRepository.UpdateAsync(venta);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al cancelar la venta: {ex.Message}", ex);
        }
    }

    public async Task<bool> CompletarVentaAsync(int id)
    {
        try
        {
            var venta = await _ventaRepository.GetByIdWithDetailsAsync(id);
            if (venta == null)
                return false;

            if (venta.Estado == "Completada")
                throw new InvalidOperationException("La venta ya está completada.");

            if (venta.Estado == "Cancelada")
                throw new InvalidOperationException("No se puede completar una venta cancelada.");

            // Descontar stock si es necesario
            foreach (var detalle in venta.Detalles)
            {
                var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                if (producto == null)
                    throw new ArgumentException($"El producto con ID {detalle.ProductoId} no existe.");

                if (producto.Stock < detalle.Cantidad)
                    throw new InvalidOperationException($"Stock insuficiente para el producto '{producto.Nombre}'.");

                producto.Stock -= detalle.Cantidad;
                await _productoRepository.UpdateAsync(producto);
            }

            venta.Estado = "Completada";
            await _ventaRepository.UpdateAsync(venta);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al completar la venta: {ex.Message}", ex);
        }
    }

    // Métodos adicionales para API
    public async Task<IEnumerable<Venta>> GetByClienteIdAsync(int clienteId)
    {
        try
        {
            Console.WriteLine($"=== VentaService.GetByClienteIdAsync ===");
            Console.WriteLine($"Buscando ventas para ClienteId: {clienteId}");
            
            var todasLasVentas = await _ventaRepository.GetAllAsync();
            Console.WriteLine($"Total de ventas en BD: {todasLasVentas.Count()}");
            
            var ventasDelCliente = todasLasVentas.Where(v => v.ClienteId == clienteId).ToList();
            Console.WriteLine($"Ventas encontradas para ClienteId {clienteId}: {ventasDelCliente.Count}");
            
            // Log de cada venta encontrada
            foreach (var venta in ventasDelCliente)
            {
                Console.WriteLine($"  - Venta ID: {venta.Id}, Fecha: {venta.FechaVenta}, Total: {venta.Total}, ClienteId: {venta.ClienteId}");
            }
            
            return ventasDelCliente;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR en GetByClienteIdAsync: {ex.Message}");
            throw new Exception($"Error al obtener ventas por cliente: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Venta>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        try
        {
            return await _ventaRepository.GetVentasByFechaRangoAsync(fechaInicio, fechaFin);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener ventas por rango de fechas: {ex.Message}", ex);
        }
    }

    public async Task<decimal> GetTotalVentasPeriodoAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        try
        {
            var ventas = await _ventaRepository.GetVentasByFechaRangoAsync(fechaInicio, fechaFin);
            return ventas.Sum(v => v.Total);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al calcular total de ventas: {ex.Message}", ex);
        }
    }
}

