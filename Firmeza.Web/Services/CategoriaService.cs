using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;

namespace Firmeza.Web.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        try
        {
            return await _categoriaRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener todas las categorías: {ex.Message}", ex);
        }
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        try
        {
            return await _categoriaRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la categoría con ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<Categoria> CreateAsync(Categoria categoria)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new ArgumentException("El nombre de la categoría es requerido.");

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                throw new ArgumentException("La descripción de la categoría es requerida.");

            var exists = await _categoriaRepository.ExistsByNombreAsync(categoria.Nombre);
            if (exists)
                throw new ArgumentException($"Ya existe una categoría con el nombre '{categoria.Nombre}'.");

            await _categoriaRepository.AddAsync(categoria);
            return categoria;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear la categoría: {ex.Message}", ex);
        }
    }

    public async Task<Categoria> UpdateAsync(Categoria categoria)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new ArgumentException("El nombre de la categoría es requerido.");

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                throw new ArgumentException("La descripción de la categoría es requerida.");

            var exists = await _categoriaRepository.ExistsAsync(categoria.Id);
            if (!exists)
                throw new ArgumentException("La categoría no existe.");

            var nombreExists = await _categoriaRepository.ExistsByNombreAsync(categoria.Nombre, categoria.Id);
            if (nombreExists)
                throw new ArgumentException($"Ya existe otra categoría con el nombre '{categoria.Nombre}'.");

            await _categoriaRepository.UpdateAsync(categoria);
            return categoria;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar la categoría: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var exists = await _categoriaRepository.ExistsAsync(id);
            if (!exists)
                return false;

            var tieneProductos = await _categoriaRepository.TieneProductosAsync(id);
            if (tieneProductos)
                throw new InvalidOperationException("No se puede eliminar la categoría porque tiene productos asociados.");

            await _categoriaRepository.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar la categoría: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            return await _categoriaRepository.ExistsAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar la existencia de la categoría: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)
    {
        try
        {
            return await _categoriaRepository.ExistsByNombreAsync(nombre, excludeId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar si existe la categoría por nombre: {ex.Message}", ex);
        }
    }

    public async Task<bool> TieneProductosAsync(int categoriaId)
    {
        try
        {
            return await _categoriaRepository.TieneProductosAsync(categoriaId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar si la categoría tiene productos: {ex.Message}", ex);
        }
    }
}

