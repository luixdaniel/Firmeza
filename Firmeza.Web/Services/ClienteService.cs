using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;

namespace Firmeza.Web.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        try
        {
            return await _clienteRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener todos los clientes: {ex.Message}", ex);
        }
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
        try
        {
            return await _clienteRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el cliente con ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<Cliente> CreateAsync(Cliente cliente)
    {
        try
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new ArgumentException("El nombre del cliente es requerido.");

            if (string.IsNullOrWhiteSpace(cliente.Apellido))
                throw new ArgumentException("El apellido del cliente es requerido.");

            if (string.IsNullOrWhiteSpace(cliente.Email))
                throw new ArgumentException("El email del cliente es requerido.");

            if (string.IsNullOrWhiteSpace(cliente.Direccion))
                throw new ArgumentException("La dirección del cliente es requerida.");

            // Verificar duplicados
            var emailExists = await _clienteRepository.ExistsByEmailAsync(cliente.Email);
            if (emailExists)
                throw new ArgumentException($"Ya existe un cliente con el email '{cliente.Email}'.");

            if (!string.IsNullOrWhiteSpace(cliente.Documento))
            {
                var documentoExists = await _clienteRepository.ExistsByDocumentoAsync(cliente.Documento);
                if (documentoExists)
                    throw new ArgumentException($"Ya existe un cliente con el documento '{cliente.Documento}'.");
            }

            cliente.FechaRegistro = DateTime.UtcNow;
            cliente.Activo = true;

            await _clienteRepository.AddAsync(cliente);
            return cliente;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear el cliente: {ex.Message}", ex);
        }
    }

    public async Task<Cliente> UpdateAsync(Cliente cliente)
    {
        try
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new ArgumentException("El nombre del cliente es requerido.");

            if (string.IsNullOrWhiteSpace(cliente.Apellido))
                throw new ArgumentException("El apellido del cliente es requerido.");

            if (string.IsNullOrWhiteSpace(cliente.Email))
                throw new ArgumentException("El email del cliente es requerido.");

            if (string.IsNullOrWhiteSpace(cliente.Direccion))
                throw new ArgumentException("La dirección del cliente es requerida.");

            var exists = await _clienteRepository.ExistsAsync(cliente.Id);
            if (!exists)
                throw new ArgumentException("El cliente no existe.");

            // Verificar duplicados excluyendo el cliente actual
            var emailExists = await _clienteRepository.ExistsByEmailAsync(cliente.Email, cliente.Id);
            if (emailExists)
                throw new ArgumentException($"Ya existe otro cliente con el email '{cliente.Email}'.");

            if (!string.IsNullOrWhiteSpace(cliente.Documento))
            {
                var documentoExists = await _clienteRepository.ExistsByDocumentoAsync(cliente.Documento, cliente.Id);
                if (documentoExists)
                    throw new ArgumentException($"Ya existe otro cliente con el documento '{cliente.Documento}'.");
            }

            await _clienteRepository.UpdateAsync(cliente);
            return cliente;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el cliente: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var exists = await _clienteRepository.ExistsAsync(id);
            if (!exists)
                return false;

            await _clienteRepository.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el cliente: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            return await _clienteRepository.ExistsAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar la existencia del cliente: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
    {
        try
        {
            return await _clienteRepository.ExistsByEmailAsync(email, excludeId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar si existe el cliente por email: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsByDocumentoAsync(string documento, int? excludeId = null)
    {
        try
        {
            return await _clienteRepository.ExistsByDocumentoAsync(documento, excludeId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al verificar si existe el cliente por documento: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Cliente>> GetClientesActivosAsync()
    {
        try
        {
            return await _clienteRepository.GetClientesActivosAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener clientes activos: {ex.Message}", ex);
        }
    }

    public async Task<Cliente?> GetByEmailAsync(string email)
    {
        try
        {
            return await _clienteRepository.GetByEmailAsync(email);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el cliente por email: {ex.Message}", ex);
        }
    }

    public async Task<bool> ActivarDesactivarAsync(int id, bool activo)
    {
        try
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
                return false;

            cliente.Activo = activo;
            await _clienteRepository.UpdateAsync(cliente);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al {(activo ? "activar" : "desactivar")} el cliente: {ex.Message}", ex);
        }
    }
}

