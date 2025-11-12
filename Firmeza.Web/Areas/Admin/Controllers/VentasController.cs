using Firmeza.Web.Areas.Admin.ViewModels;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrador")]
public class VentasController : Controller
{
    private readonly IVentaService _ventaService;
    private readonly IProductoService _productoService;
    private readonly IClienteService _clienteService;

    public VentasController(IVentaService ventaService, IProductoService productoService, IClienteService clienteService)
    {
        _ventaService = ventaService;
        _productoService = productoService;
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var ventas = await _ventaService.GetAllAsync();
            return View(ventas);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al cargar ventas: {ex.Message}";
            return View(new List<Venta>());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var venta = await _ventaService.GetByIdWithDetailsAsync(id);
            if (venta == null)
            {
                TempData["ErrorMessage"] = "Venta no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(venta);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al cargar detalles: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Create()
    {
        try
        {
            var productos = await _productoService.GetAllAsync();
            var productosDisponibles = productos.Where(p => p.Stock > 0).ToList();

            // Solo mostrar clientes activos
            var clientes = await _clienteService.GetClientesActivosAsync();

            var viewModel = new CreateVentaViewModel
            {
                ProductosDisponibles = productosDisponibles,
                Vendedor = User.Identity?.Name ?? "Sistema"
            };

            ViewBag.Clientes = clientes;
            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al cargar formulario: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // Versión simplificada para pruebas
    public async Task<IActionResult> CreateTest()
    {
        try
        {
            var productos = await _productoService.GetAllAsync();
            var productosDisponibles = productos.Where(p => p.Stock > 0).ToList();

            // Solo mostrar clientes activos
            var clientes = await _clienteService.GetClientesActivosAsync();

            var viewModel = new CreateVentaViewModel
            {
                ProductosDisponibles = productosDisponibles,
                Vendedor = User.Identity?.Name ?? "Sistema"
            };

            ViewBag.Clientes = clientes;
            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al cargar formulario: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateVentaViewModel viewModel)
    {
        try
        {
            // Log para debugging
            Console.WriteLine($"=== POST Create Ventas ===");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            Console.WriteLine($"Cliente: {viewModel.Cliente ?? "NULL"}");
            Console.WriteLine($"MetodoPago: {viewModel.MetodoPago ?? "NULL"}");
            Console.WriteLine($"Vendedor: {viewModel.Vendedor ?? "NULL"}");
            Console.WriteLine($"Detalles Count: {viewModel.Detalles?.Count ?? 0}");
            
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState INVALID:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"  Key: {error.Key}");
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"    Error: {err.ErrorMessage}");
                    }
                }
            }
            
            if (viewModel.Detalles != null)
            {
                foreach (var d in viewModel.Detalles)
                {
                    Console.WriteLine($"  - ProductoId: {d.ProductoId}, Cantidad: {d.Cantidad}, Precio: {d.PrecioUnitario}");
                }
            }

            // Filtrar detalles válidos (eliminar entradas vacías)
            if (viewModel.Detalles != null)
            {
                viewModel.Detalles = viewModel.Detalles.Where(d => d.ProductoId > 0 && d.Cantidad > 0).ToList();
                Console.WriteLine($"Detalles después de filtrar: {viewModel.Detalles.Count}");
            }

            // Validar que haya al menos un detalle
            if (viewModel.Detalles == null || viewModel.Detalles.Count == 0)
            {
                Console.WriteLine("ERROR: No hay detalles");
                TempData["ErrorMessage"] = "Debe agregar al menos un producto a la venta.";
                var productos = await _productoService.GetAllAsync();
                viewModel.ProductosDisponibles = productos.Where(p => p.Stock > 0).ToList();
                var clientes = await _clienteService.GetAllAsync();
                ViewBag.Clientes = clientes;
                return View(viewModel);
            }

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(viewModel.Cliente))
            {
                TempData["ErrorMessage"] = "Debe seleccionar un cliente.";
                var productos = await _productoService.GetAllAsync();
                viewModel.ProductosDisponibles = productos.Where(p => p.Stock > 0).ToList();
                var clientes = await _clienteService.GetAllAsync();
                ViewBag.Clientes = clientes;
                return View(viewModel);
            }

            // Validar que el cliente existe en la base de datos
            var todosLosClientes = await _clienteService.GetAllAsync();
            var clienteExiste = todosLosClientes.Any(c => c.Nombre == viewModel.Cliente);
            if (!clienteExiste)
            {
                TempData["ErrorMessage"] = "El cliente seleccionado no existe. Por favor, seleccione un cliente válido de la lista.";
                var productos = await _productoService.GetAllAsync();
                viewModel.ProductosDisponibles = productos.Where(p => p.Stock > 0).ToList();
                var clientes = await _clienteService.GetAllAsync();
                ViewBag.Clientes = clientes;
                return View(viewModel);
            }

            if (string.IsNullOrWhiteSpace(viewModel.MetodoPago))
            {
                TempData["ErrorMessage"] = "El método de pago es requerido.";
                var productos = await _productoService.GetAllAsync();
                viewModel.ProductosDisponibles = productos.Where(p => p.Stock > 0).ToList();
                var clientes = await _clienteService.GetAllAsync();
                ViewBag.Clientes = clientes;
                return View(viewModel);
            }

            // Crear la venta con sus detalles
            var venta = new Venta
            {
                Cliente = viewModel.Cliente,
                MetodoPago = viewModel.MetodoPago,
                Vendedor = viewModel.Vendedor ?? User.Identity?.Name ?? "Sistema",
                FechaVenta = DateTime.UtcNow,
                Estado = "Completada",
                Detalles = new List<DetalleDeVenta>()
            };

            // Agregar los detalles
            foreach (var detalle in viewModel.Detalles)
            {
                venta.Detalles.Add(new DetalleDeVenta
                {
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                });
            }

            // Crear la venta (esto también actualiza el stock)
            await _ventaService.CrearVentaConDetallesAsync(venta);

            TempData["SuccessMessage"] = "Venta creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al crear la venta: {ex.Message}";
            var productos = await _productoService.GetAllAsync();
            viewModel.ProductosDisponibles = productos.Where(p => p.Stock > 0).ToList();
            var clientes = await _clienteService.GetAllAsync();
            ViewBag.Clientes = clientes;
            return View(viewModel);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var venta = await _ventaService.GetByIdWithDetailsAsync(id);
            if (venta == null)
            {
                TempData["ErrorMessage"] = "Venta no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            // Las ventas completadas no deberían editarse normalmente
            // Redirigir a Details en su lugar
            TempData["InfoMessage"] = "Las ventas completadas no pueden editarse. Puede ver los detalles aquí.";
            return RedirectToAction(nameof(Details), new { id = id });
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
            var venta = await _ventaService.GetByIdWithDetailsAsync(id);
            if (venta == null)
            {
                TempData["ErrorMessage"] = "Venta no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(venta);
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
            var result = await _ventaService.DeleteAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Venta eliminada exitosamente y stock restaurado.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la venta.";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }

    // API para obtener datos de producto
    [HttpGet]
    public async Task<IActionResult> GetProducto(int id)
    {
        try
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
                return NotFound();

            return Json(new
            {
                id = producto.Id,
                nombre = producto.Nombre,
                precio = producto.Precio,
                stock = producto.Stock
            });
        }
        catch
        {
            return BadRequest();
        }
    }

    // Método de prueba para verificar POST
    [HttpPost]
    public IActionResult TestPost(string cliente, string metodoPago, List<DetalleVentaViewModel> detalles)
    {
        Console.WriteLine($"=== TEST POST ===");
        Console.WriteLine($"Cliente: {cliente}");
        Console.WriteLine($"MetodoPago: {metodoPago}");
        Console.WriteLine($"Detalles: {detalles?.Count ?? 0}");
        
        if (detalles != null)
        {
            foreach (var d in detalles)
            {
                Console.WriteLine($"  ProductoId: {d.ProductoId}, Cantidad: {d.Cantidad}");
            }
        }

        return Json(new { success = true, message = "Datos recibidos", detallesCount = detalles?.Count ?? 0 });
    }

    // Diagnóstico: verificar datos del formulario
    [HttpPost]
    public IActionResult DiagnosticoVenta([FromForm] CreateVentaViewModel viewModel)
    {
        var diagnostico = new
        {
            cliente = viewModel.Cliente,
            metodoPago = viewModel.MetodoPago,
            vendedor = viewModel.Vendedor,
            detallesCount = viewModel.Detalles?.Count ?? 0,
            detalles = viewModel.Detalles?.Select(d => new
            {
                productoId = d.ProductoId,
                cantidad = d.Cantidad,
                precioUnitario = d.PrecioUnitario
            }).ToList()
        };

        Console.WriteLine($"=== DIAGNOSTICO ===");
        Console.WriteLine($"Cliente: {viewModel.Cliente}");
        Console.WriteLine($"MetodoPago: {viewModel.MetodoPago}");
        Console.WriteLine($"Detalles: {viewModel.Detalles?.Count ?? 0}");

        return Json(diagnostico);
    }
}

