using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
namespace Firmeza.Web.Services;
public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;
    private readonly AppDbContext _context;
    public ProductoService(IProductoRepository productoRepository, AppDbContext context)
    {
        _productoRepository = productoRepository;
        _context = context;
    }
    // Implementación de IGenericService
    public async Task<IEnumerable<Producto>> GetAllAsync()
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

    public async Task<Producto?> GetByIdAsync(int id)
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

    public async Task<Producto> CreateAsync(Producto producto)
    {
        try
        {
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
            var categoriaExists = await CategoriaExistsAsync(producto.CategoriaId);
            if (!categoriaExists)
                throw new ArgumentException("La categoría seleccionada no existe.");
            await _productoRepository.AddAsync(producto);
            return producto;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear el producto: {ex.Message}", ex);
        }
    }
    public async Task<Producto> UpdateAsync(Producto producto)
    {
        try
        {
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
            var exists = await ExistsAsync(producto.Id);
            if (!exists)
                throw new ArgumentException("El producto no existe.");
            var categoriaExists = await CategoriaExistsAsync(producto.CategoriaId);
            if (!categoriaExists)
                throw new ArgumentException("La categoría seleccionada no existe.");
            await _productoRepository.UpdateAsync(producto);
            return producto;
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
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
                return false;
            await _productoRepository.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el producto: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            return await _context.Productos.AnyAsync(p => p.Id == id);
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
            return await _context.Categorias.AnyAsync(c => c.Id == categoriaId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar la existencia de la categoría: {ex.Message}", ex);
        }
    }
}
