namespace Firmeza.Web.Interfaces.Repositories;

/// <summary>
/// Interfaz base genérica para repositorios
/// </summary>
/// <typeparam name="TEntity">Tipo de entidad</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    // Consultas básicas
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    
    // Verificaciones
    Task<bool> ExistsAsync(int id);
    
    // Operaciones CRUD
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
    
    // Persistencia
    Task<int> SaveChangesAsync();
}

