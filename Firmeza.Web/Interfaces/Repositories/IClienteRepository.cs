using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Repositories;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> GetAllAsync();
    Task<Cliente?> GetByIdAsync(int id);
    Task AddAsync(Cliente cliente);
    Task UpdateAsync(Cliente cliente);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
    Task<bool> ExistsByDocumentoAsync(string documento, int? excludeId = null);
    Task<IEnumerable<Cliente>> GetClientesActivosAsync();
    Task<Cliente?> GetByEmailAsync(string email);
}

