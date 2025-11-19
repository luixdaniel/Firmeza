namespace ApiFirmeza.Web.DTOs;

public class VentaDto
{
    public int Id { get; set; }
    public DateTime FechaVenta { get; set; }
    public string NumeroFactura { get; set; } = null!;
    public string Cliente { get; set; } = null!;
    public int? ClienteId { get; set; }
    public decimal Total { get; set; }
    public decimal Subtotal { get; set; }
    public decimal IVA { get; set; }
    public string MetodoPago { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public string Vendedor { get; set; } = null!;
    public List<DetalleVentaDto> Detalles { get; set; } = new();
}

public class DetalleVentaDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = null!;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class VentaCreateDto
{
    public string Cliente { get; set; } = null!;
    public int? ClienteId { get; set; }
    public string MetodoPago { get; set; } = "Efectivo";
    public string Vendedor { get; set; } = "Sistema";
    public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
}

public class DetalleVentaCreateDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}

