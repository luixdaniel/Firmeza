using System.ComponentModel.DataAnnotations;

namespace ApiFirmeza.Web.DTOs;

public class CategoriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public int CantidadProductos { get; set; }
}

public class CategoriaCreateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
    public string Nombre { get; set; } = null!;
    
    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
    public string Descripcion { get; set; } = null!;
}

public class CategoriaUpdateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
    public string Nombre { get; set; } = null!;
    
    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
    public string Descripcion { get; set; } = null!;
}

