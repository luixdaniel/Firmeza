using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Repositories;

public class VentaRepository : IVentaRepository
{
    private readonly AppDbContext _context;

    public VentaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Venta>> GetAllAsync()
    {
        return await _context.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .AsNoTracking()
            .OrderByDescending(v => v.FechaVenta)
            .ToListAsync();
    }

    public async Task<Venta?> GetByIdAsync(int id)
    {
        return await _context.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Venta?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                    .ThenInclude(p => p.Categoria)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task AddAsync(Venta venta)
    {
        Console.WriteLine($"=== VentaRepository.AddAsync ===");
        Console.WriteLine($"Venta Cliente: {venta.Cliente}");
        Console.WriteLine($"Venta Total: {venta.Total}");
        Console.WriteLine($"Detalles count: {venta.Detalles.Count}");
        
        await _context.Ventas.AddAsync(venta);
        Console.WriteLine("Venta agregada al contexto");
        
        var cambios = await _context.SaveChangesAsync();
        Console.WriteLine($"SaveChangesAsync ejecutado. Registros afectados: {cambios}");
        Console.WriteLine($"Venta guardada con ID: {venta.Id}");
    }

    public async Task UpdateAsync(Venta venta)
    {
        _context.Ventas.Update(venta);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var venta = await _context.Ventas
            .Include(v => v.Detalles)
            .FirstOrDefaultAsync(v => v.Id == id);
        
        if (venta != null)
        {
            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Ventas.AnyAsync(v => v.Id == id);
    }

    public async Task<bool> ExistsByNumeroFacturaAsync(string numeroFactura, int? excludeId = null)
    {
        var query = _context.Ventas.Where(v => v.NumeroFactura == numeroFactura);
        
        if (excludeId.HasValue)
        {
            query = query.Where(v => v.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Venta>> GetVentasByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _context.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .Where(v => v.FechaVenta >= fechaInicio && v.FechaVenta <= fechaFin)
            .OrderByDescending(v => v.FechaVenta)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Venta>> GetVentasByEstadoAsync(string estado)
    {
        return await _context.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .Where(v => v.Estado == estado)
            .OrderByDescending(v => v.FechaVenta)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<decimal> GetTotalVentasByFechaAsync(DateTime fecha)
    {
        var fechaInicio = fecha.Date;
        var fechaFin = fechaInicio.AddDays(1);

        return await _context.Ventas
            .Where(v => v.FechaVenta >= fechaInicio && v.FechaVenta < fechaFin && v.Estado == "Completada")
            .SumAsync(v => v.Total);
    }

    public async Task<IEnumerable<Venta>> GetVentasByClienteAsync(string cliente)
    {
        return await _context.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .Where(v => v.Cliente.Contains(cliente))
            .OrderByDescending(v => v.FechaVenta)
            .AsNoTracking()
            .ToListAsync();
    }
}

