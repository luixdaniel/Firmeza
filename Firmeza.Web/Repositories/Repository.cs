using Firmeza.Web.Data;
using Firmeza.Web.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Repositories;

/// <summary>
/// Implementación base genérica para repositorios
/// </summary>
/// <typeparam name="TEntity">Tipo de entidad</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    // Consultas básicas
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    // Verificaciones
    public virtual async Task<bool> ExistsAsync(int id)
    {
        var entity = await DbSet.FindAsync(id);
        return entity != null;
    }

    // Operaciones CRUD
    public virtual async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }

    // Persistencia
    public virtual async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }
}

