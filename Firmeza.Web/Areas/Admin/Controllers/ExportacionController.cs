using Firmeza.Web.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ExportacionController : Controller
{
    private readonly IExportacionService _exportacionService;
    private readonly IPdfService _pdfService;

    public ExportacionController(IExportacionService exportacionService, IPdfService pdfService)
    {
        _exportacionService = exportacionService;
        _pdfService = pdfService;
    }

    #region Exportación de Productos

    [HttpGet]
    public async Task<IActionResult> ExportarProductosExcel()
    {
        try
        {
            var contenido = await _exportacionService.ExportarProductosExcelAsync();
            return File(contenido, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"Productos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al exportar productos a Excel: {ex.Message}";
            return RedirectToAction("Index", "Productos");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ExportarProductosPdf()
    {
        try
        {
            var contenido = await _exportacionService.ExportarProductosPdfAsync();
            return File(contenido, "application/pdf", $"Productos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al exportar productos a PDF: {ex.Message}";
            return RedirectToAction("Index", "Productos");
        }
    }

    #endregion

    #region Exportación de Clientes

    [HttpGet]
    public async Task<IActionResult> ExportarClientesExcel()
    {
        try
        {
            var contenido = await _exportacionService.ExportarClientesExcelAsync();
            return File(contenido, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"Clientes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al exportar clientes a Excel: {ex.Message}";
            return RedirectToAction("Index", "Clientes");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ExportarClientesPdf()
    {
        try
        {
            var contenido = await _exportacionService.ExportarClientesPdfAsync();
            return File(contenido, "application/pdf", $"Clientes_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al exportar clientes a PDF: {ex.Message}";
            return RedirectToAction("Index", "Clientes");
        }
    }

    #endregion

    #region Exportación de Ventas

    [HttpGet]
    public async Task<IActionResult> ExportarVentasExcel(DateTime? fechaInicio = null, DateTime? fechaFin = null)
    {
        try
        {
            var contenido = await _exportacionService.ExportarVentasExcelAsync(fechaInicio, fechaFin);
            return File(contenido, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al exportar ventas a Excel: {ex.Message}";
            return RedirectToAction("Index", "Ventas");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ExportarVentasPdf(DateTime? fechaInicio = null, DateTime? fechaFin = null)
    {
        try
        {
            var contenido = await _exportacionService.ExportarVentasPdfAsync(fechaInicio, fechaFin);
            return File(contenido, "application/pdf", $"Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al exportar ventas a PDF: {ex.Message}";
            return RedirectToAction("Index", "Ventas");
        }
    }

    #endregion

    #region Recibos PDF

    [HttpGet]
    public async Task<IActionResult> DescargarRecibo(int ventaId)
    {
        try
        {
            var rutaRelativa = await _pdfService.ObtenerRutaReciboPdfAsync(ventaId);
            
            if (string.IsNullOrEmpty(rutaRelativa))
            {
                TempData["ErrorMessage"] = "El recibo no existe o no se ha generado.";
                return RedirectToAction("Index", "Ventas");
            }

            var nombreArchivo = Path.GetFileName(rutaRelativa);
            var contenido = await _pdfService.ObtenerReciboPdfAsync(nombreArchivo);
            
            return File(contenido, "application/pdf", nombreArchivo);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al descargar el recibo: {ex.Message}";
            return RedirectToAction("Index", "Ventas");
        }
    }

    [HttpGet]
    public async Task<IActionResult> VerRecibo(int ventaId)
    {
        try
        {
            var rutaRelativa = await _pdfService.ObtenerRutaReciboPdfAsync(ventaId);
            
            if (string.IsNullOrEmpty(rutaRelativa))
            {
                TempData["ErrorMessage"] = "El recibo no existe o no se ha generado.";
                return RedirectToAction("Index", "Ventas");
            }

            var nombreArchivo = Path.GetFileName(rutaRelativa);
            var contenido = await _pdfService.ObtenerReciboPdfAsync(nombreArchivo);
            
            return File(contenido, "application/pdf");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al ver el recibo: {ex.Message}";
            return RedirectToAction("Index", "Ventas");
        }
    }

    #endregion
}

