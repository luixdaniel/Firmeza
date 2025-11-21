using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Services;

public interface IVentaService : IGenericService<Venta>
{
    Task<Venta?> GetByIdWithDetailsAsync(int id);
    Task<bool> ExistsByNumeroFacturaAsync(string numeroFactura, int? excludeId = null);
    Task<IEnumerable<Venta>> GetVentasByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<IEnumerable<Venta>> GetVentasByEstadoAsync(string estado);
    Task<decimal> GetTotalVentasByFechaAsync(DateTime fecha);
    Task<IEnumerable<Venta>> GetVentasByClienteAsync(string cliente);
    Task<Venta> CrearVentaConDetallesAsync(Venta venta);
    Task<bool> CancelarVentaAsync(int id);
    Task<bool> CompletarVentaAsync(int id);
    
    // Métodos adicionales para API
    Task<IEnumerable<Venta>> GetByClienteIdAsync(int clienteId);
    Task<IEnumerable<Venta>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<decimal> GetTotalVentasPeriodoAsync(DateTime fechaInicio, DateTime fechaFin);
}

