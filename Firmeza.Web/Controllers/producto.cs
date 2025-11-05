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
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Precio,Stock,CategoriaId")] Producto producto)
        {
            try
            {
                // Validaciones adicionales
                if (string.IsNullOrWhiteSpace(producto.Nombre))
                {
                    ModelState.AddModelError("Nombre", "El nombre es requerido.");
                }

                if (string.IsNullOrWhiteSpace(producto.Descripcion))
                {
                    ModelState.AddModelError("Descripcion", "La descripción es requerida.");
                }

                if (producto.Precio <= 0)
                {
                    ModelState.AddModelError("Precio", "El precio debe ser mayor a 0.");
                }

                if (producto.Stock < 0)
                {
                    ModelState.AddModelError("Stock", "El stock no puede ser negativo.");
                }

                if (producto.CategoriaId <= 0)
                {
                    ModelState.AddModelError("CategoriaId", "Debe seleccionar una categoría.");
                }

                if (!ModelState.IsValid)
                {
                    ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                    return View(model: producto);
                }

                // Verificar que la categoría existe
                var categoriaExists = await _context.Set<Categoria>().AnyAsync(c => c.Id == producto.CategoriaId);
                if (!categoriaExists)
                {
                    TempData["Error"] = "La categoría seleccionada no existe.";
                    ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                    return View(model: producto);
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["Error"] = $"No se pudo guardar el producto. Error: {ex.InnerException?.Message}";
                ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                return View(model: producto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error inesperado: {ex.Message}";
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

            // Validaciones adicionales
            if (string.IsNullOrWhiteSpace(producto.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre es requerido.");
            }

            if (string.IsNullOrWhiteSpace(producto.Descripcion))
            {
                ModelState.AddModelError("Descripcion", "La descripción es requerida.");
            }

            if (producto.Precio <= 0)
            {
                ModelState.AddModelError("Precio", "El precio debe ser mayor a 0.");
            }

            if (producto.Stock < 0)
            {
                ModelState.AddModelError("Stock", "El stock no puede ser negativo.");
            }

            if (producto.CategoriaId <= 0)
            {
                ModelState.AddModelError("CategoriaId", "Debe seleccionar una categoría.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                return View(model: producto);
            }

            try
            {
                // Verificar que la categoría existe
                var categoriaExists = await _context.Set<Categoria>().AnyAsync(c => c.Id == producto.CategoriaId);
                if (!categoriaExists)
                {
                    TempData["Error"] = "La categoría seleccionada no existe.";
                    ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                    return View(model: producto);
                }

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
            catch (DbUpdateException ex)
            {
                TempData["Error"] = $"No se pudo actualizar el producto. Error: {ex.InnerException?.Message}";
                ViewData["CategoriaId"] = new SelectList(_context.Set<Categoria>(), "Id", "Nombre", producto.CategoriaId);
                return View(model: producto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error inesperado: {ex.Message}";
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
