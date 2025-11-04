using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrador")]
public class ClientesController : Controller
{
    private readonly UserManager<Firmeza.Web.Identity.ApplicationUser> _userManager;

    public ClientesController(UserManager<Firmeza.Web.Identity.ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var clientes = await _userManager.GetUsersInRoleAsync("Cliente");
            return View(clientes);
        }
        catch (Exception)
        {
            TempData["Error"] = "No se pudieron cargar los clientes. Intenta nuevamente.";
            return View(Enumerable.Empty<Firmeza.Web.Identity.ApplicationUser>());
        }
    }
}
