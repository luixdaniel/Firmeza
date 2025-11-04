using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrador")]
public class VentasController : Controller
{
    private readonly AppDbContext _context;

    public VentasController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var ventas = await _context.Ventas.AsNoTracking().ToListAsync();
        return View(ventas);
    }

    public async Task<IActionResult> Details(int id)
    {
        var venta = await _context.Ventas
            .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (venta == null) return NotFound();
        return View(venta);
    }

    public IActionResult Create()
    {
        return View(new Venta { FechaVenta = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Venta venta)
    {
        if (!ModelState.IsValid) return View(venta);
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var venta = await _context.Ventas.FindAsync(id);
        if (venta == null) return NotFound();
        return View(venta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Venta venta)
    {
        if (id != venta.Id) return NotFound();
        if (!ModelState.IsValid) return View(venta);
        _context.Update(venta);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var venta = await _context.Ventas.FindAsync(id);
        if (venta == null) return NotFound();
        return View(venta);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var venta = await _context.Ventas.FindAsync(id);
        if (venta != null)
        {
            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}

