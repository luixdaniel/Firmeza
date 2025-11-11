using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Services;

public interface IProductoService : IGenericService<Producto>
{
    // Métodos específicos adicionales para productos
    Task<bool> CategoriaExistsAsync(int categoriaId);
}
