using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Areas.Admin.ViewModels;

public class EditProductViewModel
{
    [Required(ErrorMessage = "El ID es requerido")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    [Display(Name = "Nombre del Producto")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ0-9\s\-\.,]+$", ErrorMessage = "El nombre contiene caracteres inválidos")]
    public string Nombre { get; set; } = null!;
    
    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 500 caracteres")]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = null!;
    
    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio debe estar entre 0.01 y 999,999.99")]
    [Display(Name = "Precio")]
    [DataType(DataType.Currency)]
    public decimal Precio { get; set; }
    
    [Required(ErrorMessage = "El stock es requerido")]
    [Range(0, 999999, ErrorMessage = "El stock debe estar entre 0 y 999,999")]
    [Display(Name = "Stock")]
    public int Stock { get; set; }

    // Relaciones
    [Required(ErrorMessage = "La categoría es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida")]
    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }
}

