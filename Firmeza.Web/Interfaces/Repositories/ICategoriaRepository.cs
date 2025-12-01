using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Repositories;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task AddAsync(Categoria categoria);
    Task UpdateAsync(Categoria categoria);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null);
    Task<bool> TieneProductosAsync(int categoriaId);
}

