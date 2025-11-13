namespace Firmeza.Web.Interfaces.Services;

public interface IExportacionService
{
    /// <summary>
    /// Exporta productos a Excel
    /// </summary>
    Task<byte[]> ExportarProductosExcelAsync();
    
    /// <summary>
    /// Exporta clientes a Excel
    /// </summary>
    Task<byte[]> ExportarClientesExcelAsync();
    
    /// <summary>
    /// Exporta ventas a Excel
    /// </summary>
    Task<byte[]> ExportarVentasExcelAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null);
    
    /// <summary>
    /// Exporta productos a PDF
    /// </summary>
    Task<byte[]> ExportarProductosPdfAsync();
    
    /// <summary>
    /// Exporta clientes a PDF
    /// </summary>
    Task<byte[]> ExportarClientesPdfAsync();
    
    /// <summary>
    /// Exporta ventas a PDF
    /// </summary>
    Task<byte[]> ExportarVentasPdfAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null);
}

