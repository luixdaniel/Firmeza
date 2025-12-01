using System.ComponentModel.DataAnnotations;

namespace ApiFirmeza.Web.DTOs;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }
    public string? ImagenUrl { get; set; }
    public bool Activo { get; set; }
}

public class ProductoCreateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
    public string Nombre { get; set; } = null!;
    
    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
    public string Descripcion { get; set; } = null!;
    
    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }
    
    [Required(ErrorMessage = "El stock es requerido")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }
    
    [Required(ErrorMessage = "La categoría es requerida")]
    public int CategoriaId { get; set; }
}

public class ProductoUpdateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
    public string Nombre { get; set; } = null!;
    
    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
    public string Descripcion { get; set; } = null!;
    
    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }
    
    [Required(ErrorMessage = "El stock es requerido")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }
    
    [Required(ErrorMessage = "La categoría es requerida")]
    public int CategoriaId { get; set; }
}

