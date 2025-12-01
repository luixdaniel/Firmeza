using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _context.Set<Producto>()
            .Include(p => p.Categoria)
            .ToListAsync();
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
        return await _context.Set<Producto>()
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Producto producto)
    {
        await _context.Set<Producto>().AddAsync(producto);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Producto producto)
    {
        _context.Set<Producto>().Update(producto);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<Producto>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<Producto>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}