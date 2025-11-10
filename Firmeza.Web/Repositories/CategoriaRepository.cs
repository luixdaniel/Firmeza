using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Repositories;

/// <summary>
/// Repositorio de Categorías, hereda funcionalidad base de Repository genérico
/// </summary>
public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    // Consultas específicas
    public async Task<Categoria?> GetByNombreAsync(string nombre)
    {
        return await Context.Categorias
            .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.ToLower());
    }

    public async Task<IEnumerable<Categoria>> GetCategoriasConProductosAsync()
    {
        return await Context.Categorias
            .Include(c => c.Productos)
            .AsNoTracking()
            .ToListAsync();
    }

    // Verificaciones específicas
    public async Task<bool> NombreExistsAsync(string nombre, int? excludeId = null)
    {
        var query = Context.Categorias.Where(c => c.Nombre.ToLower() == nombre.ToLower());
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<bool> TieneProductosAsync(int categoriaId)
    {
        return await Context.Productos.AnyAsync(p => p.CategoriaId == categoriaId);
    }
}

