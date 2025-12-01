using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Repositories;

/// <summary>
/// Repositorio de Categorías
/// </summary>
public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await _context.Categorias
            .Include(c => c.Productos)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _context.Categorias
            .Include(c => c.Productos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Categorias.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)
    {
        var query = _context.Categorias.Where(c => c.Nombre.ToLower() == nombre.ToLower());
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    // Métodos adicionales específicos
    public async Task<Categoria?> GetByNombreAsync(string nombre)
    {
        return await _context.Categorias
            .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.ToLower());
    }

    public async Task<bool> TieneProductosAsync(int categoriaId)
    {
        return await _context.Productos.AnyAsync(p => p.CategoriaId == categoriaId);
    }
}

