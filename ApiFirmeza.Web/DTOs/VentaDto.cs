using System.ComponentModel.DataAnnotations;

namespace ApiFirmeza.Web.DTOs;

public class VentaDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public decimal Total { get; set; }
    public List<DetalleVentaDto> Detalles { get; set; } = new();
}

public class DetalleVentaDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class VentaCreateDto
{
    [Required(ErrorMessage = "El cliente es requerido")]
    public int ClienteId { get; set; }
    
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    
    [Required(ErrorMessage = "Debe incluir al menos un detalle de venta")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un producto")]
    public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
}

public class DetalleVentaCreateDto
{
    [Required(ErrorMessage = "El producto es requerido")]
    public int ProductoId { get; set; }
    
    [Required(ErrorMessage = "La cantidad es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
    public int Cantidad { get; set; }
    
    [Required(ErrorMessage = "El precio unitario es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal PrecioUnitario { get; set; }
}

public class VentaUpdateDto
{
    [Required(ErrorMessage = "El cliente es requerido")]
    public int ClienteId { get; set; }
    
    public DateTime Fecha { get; set; }
    
    [Required(ErrorMessage = "Debe incluir al menos un detalle de venta")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un producto")]
    public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
}

