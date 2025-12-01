namespace Firmeza.Web.Models.ImportacionMasiva;

/// <summary>
/// Representa una fila de datos desnormalizados del Excel que puede contener
/// información de múltiples entidades (Producto, Cliente, Venta)
/// </summary>
public class DatosDesnormalizados
{
    public int NumeroFila { get; set; }
    
    // Datos de Producto
    public string? CodigoProducto { get; set; }
    public string? NombreProducto { get; set; }
    public string? DescripcionProducto { get; set; }
    public decimal? PrecioProducto { get; set; }
    public int? StockProducto { get; set; }
    public string? CategoriaProducto { get; set; }
    
    // Datos de Cliente
    public string? CodigoCliente { get; set; }
    public string? NombreCliente { get; set; }
    public string? ApellidoCliente { get; set; }
    public string? EmailCliente { get; set; }
    public string? TelefonoCliente { get; set; }
    public string? DireccionCliente { get; set; }
    public string? DocumentoCliente { get; set; }
    
    // Datos de Venta
    public string? NumeroFactura { get; set; }
    public DateTime? FechaVenta { get; set; }
    public int? CantidadVendida { get; set; }
    public decimal? PrecioUnitarioVenta { get; set; }
    public string? MetodoPago { get; set; }
    public string? EstadoVenta { get; set; }
    
    // Flags para saber qué tipo de datos contiene esta fila
    public bool TieneDatosProducto { get; set; }
    public bool TieneDatosCliente { get; set; }
    public bool TieneDatosVenta { get; set; }
}

