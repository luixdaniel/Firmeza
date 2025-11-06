using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firmeza.Web.Data;
using Firmeza.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Firmeza.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Productos
        public async Task<IActionResult> Index()
        {
            try
            {
                var list = await _context.Productos.Include(p => p.Categoria).ToListAsync();
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudieron cargar los productos: {ex.Message}";
                return View(new List<Producto>());
            }
        }

        // GET: Admin/Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var producto = await _context.Productos
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudo cargar el detalle del producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/Productos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Admin/Productos/Create
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
                    ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
                    return View(producto);
                }

                // Verificar que la categoría existe
                var categoriaExists = await _context.Categorias.AnyAsync(c => c.Id == producto.CategoriaId);
                if (!categoriaExists)
                {
                    TempData["Error"] = "La categoría seleccionada no existe.";
                    ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
                    return View(producto);
                }

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["Error"] = $"No se pudo guardar el producto. Error: {ex.InnerException?.Message}";
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error inesperado: {ex.Message}";
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
                return View(producto);
            }
        }

        // GET: Admin/Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudo cargar el producto para editar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock,CategoriaId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            try
            {
                // Validaciones
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
                    ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
                    return View(producto);
                }

                _context.Productos.Update(producto);
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
                TempData["Error"] = "El producto fue modificado por otro usuario. Intenta nuevamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error inesperado: {ex.Message}";
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
                return View(producto);
            }
        }

        // GET: Admin/Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var producto = await _context.Productos
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudo cargar el producto para eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto != null)
                {
                    _context.Productos.Remove(producto);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Producto eliminado correctamente.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudo eliminar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}

