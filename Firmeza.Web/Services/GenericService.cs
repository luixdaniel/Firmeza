
using Firmeza.Web.Data;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Services;

public class GenericService<T> : IGenericService<T> where T : class
{
    protected readonly IGenericRepository<T> _repository;
    protected readonly AppDbContext _context;

    public GenericService(IGenericRepository<T> repository, AppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener todos los registros: {ex.Message}", ex);
        }
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el registro con ID {id}: {ex.Message}", ex);
        }
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        try
        {
            await _repository.AddAsync(entity);
            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear el registro: {ex.Message}", ex);
        }
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        try
        {
            await _repository.UpdateAsync(entity);
            return entity;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Exception("El registro fue modificado por otro usuario. Intenta nuevamente.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el registro: {ex.Message}", ex);
        }
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el registro: {ex.Message}", ex);
        }
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        try
        {
            return await _repository.ExistsAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar la existencia del registro: {ex.Message}", ex);
        }
    }
}

