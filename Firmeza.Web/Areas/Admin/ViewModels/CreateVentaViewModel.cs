using System.ComponentModel.DataAnnotations;
using Firmeza.Web.Data.Entities;

namespace Firmeza.Web.Areas.Admin.ViewModels;

public class CreateVentaViewModel
{
    public string Cliente { get; set; } = string.Empty;

    public string MetodoPago { get; set; } = string.Empty;

    public string? Vendedor { get; set; }

    // Lista de productos disponibles para seleccionar
    public List<Producto> ProductosDisponibles { get; set; } = new();

    // Detalles de la venta (productos seleccionados)
    public List<DetalleVentaViewModel> Detalles { get; set; } = new();
}

public class DetalleVentaViewModel
{
    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string? NombreProducto { get; set; }
    public int StockDisponible { get; set; }
}

