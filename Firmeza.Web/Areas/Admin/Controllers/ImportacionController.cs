using Firmeza.Web.Interfaces.Services;
using Firmeza.Web.Models.ImportacionMasiva;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ImportacionController : Controller
{
    private readonly IImportacionMasivaService _importacionService;

    public ImportacionController(IImportacionMasivaService importacionService)
    {
        _importacionService = importacionService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportarExcel(IFormFile archivo, string tipoImportacion = "Auto")
    {
        try
        {
            if (archivo?.Length == 0)
            {
                TempData["ErrorMessage"] = "Por favor seleccione un archivo Excel válido.";
                return RedirectToAction(nameof(Index));
            }

            if (!Path.GetExtension(archivo!.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "El archivo debe ser un Excel (.xlsx).";
                return RedirectToAction(nameof(Index));
            }

            using (var stream = archivo.OpenReadStream())
            {
                var resultado = await _importacionService.ImportarDesdeExcelAsync(stream, tipoImportacion);

                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                if (resultado.Exitoso)
                {
                    TempData["SuccessMessage"] = resultado.Mensaje;
                    TempData["ImportResultado"] = System.Text.Json.JsonSerializer.Serialize(resultado, options);
                    return RedirectToAction(nameof(Resultado));
                }
                else
                {
                    TempData["ErrorMessage"] = resultado.Mensaje;
                    TempData["ImportResultado"] = System.Text.Json.JsonSerializer.Serialize(resultado, options);
                    return RedirectToAction(nameof(Resultado));
                }
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al importar archivo: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Resultado()
    {
        if (TempData["ImportResultado"] is string resultadoJson)
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var resultado = System.Text.Json.JsonSerializer.Deserialize<ImportResultado>(resultadoJson, options);
            return View(resultado);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DescargarPlantilla(string tipo = "Completa")
    {
        try
        {
            var plantilla = await _importacionService.GenerarPlantillaExcelAsync(tipo);
            var nombreArchivo = $"Plantilla_{tipo}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            
            return File(plantilla, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al generar plantilla: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}

