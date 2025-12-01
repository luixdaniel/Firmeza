using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Interfaces.Services;

public interface IPdfService
{
    /// <summary>
    /// Genera un recibo PDF para una venta y lo guarda en el sistema
    /// </summary>
    /// <param name="venta">La venta para la cual generar el recibo</param>
    /// <returns>Ruta relativa del archivo guardado</returns>
    Task<string> GenerarReciboPdfAsync(Venta venta);
    
    /// <summary>
    /// Obtiene el contenido de un recibo PDF existente
    /// </summary>
    Task<byte[]> ObtenerReciboPdfAsync(string nombreArchivo);
    
    /// <summary>
    /// Verifica si existe un recibo para una venta
    /// </summary>
    Task<bool> ExisteReciboPdfAsync(int ventaId);
    
    /// <summary>
    /// Obtiene la ruta del recibo de una venta
    /// </summary>
    Task<string?> ObtenerRutaReciboPdfAsync(int ventaId);
}

