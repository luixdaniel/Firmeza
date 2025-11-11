using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Services;

public interface ICategoriaService : IGenericService<Categoria>
{
    Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null);
    Task<bool> TieneProductosAsync(int categoriaId);
}

