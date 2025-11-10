using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public ProductoService(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
    }
    public async Task<IEnumerable<Producto>> GetAllProductosAsync()
    {
        try
        {
            return await _productoRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener todos los productos: {ex.Message}", ex);
        }
    }
    public async Task<Producto?> GetProductoByIdAsync(int id)
    {
        try
        {
            return await _productoRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el producto con ID {id}: {ex.Message}", ex);
        }
    }
    public async Task<bool> CreateProductoAsync(Producto producto)
    {
        try
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("El nombre del producto es requerido.");
            
            if (string.IsNullOrWhiteSpace(producto.Descripcion))
                throw new ArgumentException("La descripción del producto es requerida.");
            
            if (producto.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");
            
            if (producto.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");
            
            if (producto.CategoriaId <= 0)
                throw new ArgumentException("Debe seleccionar una categoría válida.");
            
            // Verificar que la categoría existe
            var categoriaExists = await CategoriaExistsAsync(producto.CategoriaId);
            if (!categoriaExists)
                throw new ArgumentException("La categoría seleccionada no existe.");
            
            // Verificar que el nombre no exista
            var nombreExists = await _productoRepository.NombreExistsAsync(producto.Nombre);
            if (nombreExists)
                throw new ArgumentException($"Ya existe un producto con el nombre '{producto.Nombre}'.");
            
            // Agregar y guardar
            await _productoRepository.AddAsync(producto);
            await _productoRepository.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear el producto: {ex.Message}", ex);
        }
    }
    public async Task<bool> UpdateProductoAsync(Producto producto)
    {
        try
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("El nombre del producto es requerido.");
            
            if (string.IsNullOrWhiteSpace(producto.Descripcion))
                throw new ArgumentException("La descripción del producto es requerida.");
            
            if (producto.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");
            
            if (producto.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");
            
            if (producto.CategoriaId <= 0)
                throw new ArgumentException("Debe seleccionar una categoría válida.");
            
            // Verificar que el producto existe
            var exists = await _productoRepository.ExistsAsync(producto.Id);
            if (!exists)
                throw new ArgumentException("El producto no existe.");
            
            // Verificar que la categoría existe
            var categoriaExists = await CategoriaExistsAsync(producto.CategoriaId);
            if (!categoriaExists)
                throw new ArgumentException("La categoría seleccionada no existe.");
            
            // Verificar que el nombre no exista (excluyendo el producto actual)
            var nombreExists = await _productoRepository.NombreExistsAsync(producto.Nombre, producto.Id);
            if (nombreExists)
                throw new ArgumentException($"Ya existe otro producto con el nombre '{producto.Nombre}'.");
            
            // Actualizar y guardar
            await _productoRepository.UpdateAsync(producto);
            await _productoRepository.SaveChangesAsync();
            
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Exception("El producto fue modificado por otro usuario. Intenta nuevamente.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el producto: {ex.Message}", ex);
        }
    }
    public async Task<bool> DeleteProductoAsync(int id)
    {
        try
        {
            // Verificar que el producto existe
            var exists = await _productoRepository.ExistsAsync(id);
            if (!exists)
                return false;
            
            // Eliminar y guardar
            await _productoRepository.DeleteAsync(id);
            await _productoRepository.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el producto: {ex.Message}", ex);
        }
    }
    public async Task<bool> ProductoExistsAsync(int id)
    {
        try
        {
            return await _productoRepository.ExistsAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar la existencia del producto: {ex.Message}", ex);
        }
    }
    public async Task<bool> CategoriaExistsAsync(int categoriaId)
    {
        try
        {
            return await _categoriaRepository.ExistsAsync(categoriaId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar la existencia de la categoría: {ex.Message}", ex);
        }
    }
}
