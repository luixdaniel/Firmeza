using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Services;

public interface IClienteService : IGenericService<Cliente>
{
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
    Task<bool> ExistsByDocumentoAsync(string documento, int? excludeId = null);
    Task<IEnumerable<Cliente>> GetClientesActivosAsync();
    Task<Cliente?> GetByEmailAsync(string email);
    Task<bool> ActivarDesactivarAsync(int id, bool activo);
    Task<Cliente?> GetByIdWithVentasAsync(int id);
}

