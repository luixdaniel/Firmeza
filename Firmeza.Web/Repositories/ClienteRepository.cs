using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _context.Clientes
            .Include(c => c.Ventas)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
        return await _context.Clientes
            .Include(c => c.Ventas)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Clientes.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
    {
        var query = _context.Clientes.Where(c => c.Email.ToLower() == email.ToLower());
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByDocumentoAsync(string documento, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(documento))
            return false;

        var query = _context.Clientes.Where(c => c.Documento == documento);
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Cliente>> GetClientesActivosAsync()
    {
        return await _context.Clientes
            .Where(c => c.Activo)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Cliente?> GetByEmailAsync(string email)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
    }
}

