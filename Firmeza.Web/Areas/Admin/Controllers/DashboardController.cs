using Firmeza.Web.Data;
using Firmeza.Web.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrador")]
public class DashboardController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<Firmeza.Web.Identity.ApplicationUser> _userManager;

    public DashboardController(AppDbContext db, UserManager<Firmeza.Web.Identity.ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var totalProductos = await _db.Productos.CountAsync();

        // Obtener cantidad de clientes (usuarios en rol "Cliente") de forma directa
        var clientes = await _userManager.GetUsersInRoleAsync("Cliente");
        var totalClientes = clientes.Count;

        var totalVentas = await _db.Ventas.CountAsync();

        var vm = new AdminDashboardViewModel
        {
            TotalProductos = totalProductos,
            TotalClientes = totalClientes,
            TotalVentas = totalVentas
        };
        return View(vm);
    }
}
