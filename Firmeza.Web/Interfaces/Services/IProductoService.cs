using Firmeza.Web.Data.Entities;
namespace Firmeza.Web.Interfaces.Services;
public interface IProductoService
{
    Task<IEnumerable<Producto>> GetAllProductosAsync();
    Task<Producto?> GetProductoByIdAsync(int id);
    Task<bool> CreateProductoAsync(Producto producto);
    Task<bool> UpdateProductoAsync(Producto producto);
    Task<bool> DeleteProductoAsync(int id);
    Task<bool> ProductoExistsAsync(int id);
    Task<bool> CategoriaExistsAsync(int categoriaId);
}
