using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Firmeza.Web.Data;
using Firmeza.Web.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Firmeza.Web.Areas.Admin.ViewModels;

namespace Firmeza.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly AppDbContext _context;

        public ProductosController(IProductoService productoService, AppDbContext context)
        {
            _productoService = productoService;
            _context = context;
        }

        // GET: Admin/Productos
        public async Task<IActionResult> Index()
        {
            try
            {
                var productos = await _productoService.GetAllAsync();
                var viewModels = productos.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria?.Nombre ?? "Sin categoría"
                }).ToList();
                
                return View(viewModels  );
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudieron cargar los productos: {ex.Message}";
                return View(new List<ProductViewModel>());
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
                var producto = await _productoService.GetByIdAsync(id.Value);
                if (producto == null)
                {
                    return NotFound();
                }
                
                var viewModel = new ProductViewModel
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    CategoriaId = producto.CategoriaId,
                    CategoriaNombre = producto.Categoria?.Nombre ?? "Sin categoría"
                };
                
                return View(viewModel);
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
        public async Task<IActionResult> Create(CreateProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", viewModel.CategoriaId);
                return View(viewModel);
            }
            
            try
            {
                var producto = new Data.Entities.Producto
                {
                    Nombre = viewModel.Nombre,
                    Descripcion = viewModel.Descripcion,
                    Precio = viewModel.Precio,
                    Stock = viewModel.Stock,
                    CategoriaId = viewModel.CategoriaId
                };
                
                await _productoService.CreateAsync(producto);
                TempData["Success"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                // Errores de validación
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", viewModel.CategoriaId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudo crear el producto: {ex.Message}";
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", viewModel.CategoriaId);
                return View(viewModel);
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
                var producto = await _productoService.GetByIdAsync(id.Value);
                if (producto == null)
                {
                    return NotFound();
                }
                
                var viewModel = new EditProductViewModel
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    CategoriaId = producto.CategoriaId
                };
                
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", viewModel.CategoriaId);
                return View(viewModel);
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
        public async Task<IActionResult> Edit(int id, EditProductViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", viewModel.CategoriaId);
                return View(viewModel);
            }

            try
            {
                var producto = new Data.Entities.Producto
                {
                    Id = viewModel.Id,
                    Nombre = viewModel.Nombre,
                    Descripcion = viewModel.Descripcion,
                    Precio = viewModel.Precio,
                    Stock = viewModel.Stock,
                    CategoriaId = viewModel.CategoriaId
                };
                
                await _productoService.UpdateAsync(producto);
                TempData["Success"] = "Producto actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                // Errores de validación
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", viewModel.CategoriaId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudo actualizar el producto: {ex.Message}";
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", viewModel.CategoriaId);
                return View(viewModel);
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
                var producto = await _productoService.GetByIdAsync(id.Value);
                if (producto == null)
                {
                    return NotFound();
                }
                
                var viewModel = new ProductViewModel
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    CategoriaId = producto.CategoriaId,
                    CategoriaNombre = producto.Categoria?.Nombre ?? "Sin categoría"
                };
                
                return View(viewModel);
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
                var resultado = await _productoService.DeleteAsync(id);
                if (resultado)
                {
                    TempData["Success"] = "Producto eliminado correctamente.";
                }
                else
                {
                    TempData["Error"] = "El producto no existe.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudo eliminar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

