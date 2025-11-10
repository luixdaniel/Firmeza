using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Repositories;

/// <summary>
/// Repositorio específico para Categorías, hereda de IRepository genérico
/// </summary>
public interface ICategoriaRepository : IRepository<Categoria>
{
    // Consultas específicas de Categoría
    Task<Categoria?> GetByNombreAsync(string nombre);
    Task<IEnumerable<Categoria>> GetCategoriasConProductosAsync();
    
    // Verificaciones específicas
    Task<bool> NombreExistsAsync(string nombre, int? excludeId = null);
    Task<bool> TieneProductosAsync(int categoriaId);
}

