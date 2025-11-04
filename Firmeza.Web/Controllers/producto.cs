using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly AppDbContext _context;

        public ProductoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            try
            {
                var query = _context.Set<Producto>().Include(p => p.Categoria);
                var list = await query.ToListAsync();
                return View(model: list);
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudieron cargar los productos. Intenta nuevamente.";
                return View(model: new List<Producto>());
            }
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var producto = await _context.Set<Producto>()
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(model: producto);
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo cargar el detalle del producto.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre");
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Stock,CategoriaId")] Producto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                    return View(model: producto);
                }
                _context.Add(producto);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "No se pudo guardar el producto. Verifica los datos e intenta nuevamente.";
                ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                return View(model: producto);
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado al crear el producto.";
                ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                return View(model: producto);
            }
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var producto = await _context.Set<Producto>().FindAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                return View(model: producto);
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo cargar el producto a editar.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock,CategoriaId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                return View(model: producto);
            }

            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Producto actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.Id))
                {
                    return NotFound();
                }
                else
                {
                    TempData["Error"] = "Conflicto de concurrencia al actualizar el producto.";
                    throw;
                }
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "No se pudo actualizar el producto.";
                ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                return View(model: producto);
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error inesperado al actualizar el producto.";
                ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                return View(model: producto);
            }
        }

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var producto = await _context.Set<Producto>()
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (producto == null)
                {
                    return NotFound();
                }

                return View(model: producto);
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo cargar el producto a eliminar.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var producto = await _context.Set<Producto>().FindAsync(id);
                if (producto != null)
                {
                    _context.Set<Producto>().Remove(producto);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Producto eliminado.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo eliminar el producto.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProductoExists(int id)
        {
            return _context.Set<Producto>().Any(e => e.Id == id);
        }
    }
}
