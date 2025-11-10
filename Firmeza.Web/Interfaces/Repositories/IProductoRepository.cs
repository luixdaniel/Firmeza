using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Repositories;

/// <summary>
/// Repositorio específico para Productos, hereda de IRepository genérico
/// </summary>
public interface IProductoRepository : IRepository<Producto>
{
    // Consultas específicas de Producto
    Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId);
    Task<IEnumerable<Producto>> GetByStockBajoAsync(int minStock = 10);
    Task<IEnumerable<Producto>> SearchByNombreAsync(string nombre);
    
    // Verificaciones específicas de Producto
    Task<bool> NombreExistsAsync(string nombre, int? excludeId = null);
}