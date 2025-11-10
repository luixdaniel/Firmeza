using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Repositories;

/// <summary>
/// Repositorio de Productos, hereda funcionalidad base de Repository genérico
/// </summary>
public class ProductoRepository : Repository<Producto>, IProductoRepository
{
    public ProductoRepository(AppDbContext context) : base(context)
    {
    }

    // Sobrescribir métodos base para incluir relaciones
    public override async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await Context.Productos
            .Include(p => p.Categoria)
            .AsNoTracking()
            .ToListAsync();
    }

    public override async Task<Producto?> GetByIdAsync(int id)
    {
        return await Context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Consultas específicas
    public async Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId)
    {
        return await Context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Producto>> GetByStockBajoAsync(int minStock = 10)
    {
        return await Context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.Stock <= minStock)
            .OrderBy(p => p.Stock)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Producto>> SearchByNombreAsync(string nombre)
    {
        return await Context.Productos
            .Include(p => p.Categoria)
            .Where(p => EF.Functions.Like(p.Nombre, $"%{nombre}%"))
            .AsNoTracking()
            .ToListAsync();
    }

    // Verificaciones específicas
    public async Task<bool> NombreExistsAsync(string nombre, int? excludeId = null)
    {
        var query = Context.Productos.Where(p => p.Nombre.ToLower() == nombre.ToLower());
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }
}