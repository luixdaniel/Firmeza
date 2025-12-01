using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ClientesController : Controller
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var clientes = await _clienteService.GetAllAsync();
            return View(clientes);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al cargar clientes: {ex.Message}";
            return View(Enumerable.Empty<Cliente>());
        }
    }

    public IActionResult Create()
    {
        return View(new Cliente());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            await _clienteService.CreateAsync(cliente);
            TempData["SuccessMessage"] = "Cliente creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al crear el cliente: {ex.Message}";
            return View(cliente);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente cliente)
    {
        try
        {
            if (id != cliente.Id)
            {
                TempData["ErrorMessage"] = "ID no válido.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            await _clienteService.UpdateAsync(cliente);
            TempData["SuccessMessage"] = "Cliente actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al actualizar: {ex.Message}";
            return View(cliente);
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var cliente = await _clienteService.GetByIdWithVentasAsync(id);
            if (cliente == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var result = await _clienteService.DeleteAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Cliente eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar el cliente.";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActivo(int id)
    {
        try
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
            {
                return Json(new { success = false, message = "Cliente no encontrado" });
            }

            await _clienteService.ActivarDesactivarAsync(id, !cliente.Activo);
            return Json(new { success = true, activo = !cliente.Activo });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
}
