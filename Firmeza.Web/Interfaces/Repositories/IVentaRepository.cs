using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Repositories;

public interface IVentaRepository
{
    Task<IEnumerable<Venta>> GetAllAsync();
    Task<Venta?> GetByIdAsync(int id);
    Task<Venta?> GetByIdWithDetailsAsync(int id);
    Task AddAsync(Venta venta);
    Task UpdateAsync(Venta venta);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNumeroFacturaAsync(string numeroFactura, int? excludeId = null);
    Task<IEnumerable<Venta>> GetVentasByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<IEnumerable<Venta>> GetVentasByEstadoAsync(string estado);
    Task<decimal> GetTotalVentasByFechaAsync(DateTime fecha);
    Task<IEnumerable<Venta>> GetVentasByClienteAsync(string cliente);
}

