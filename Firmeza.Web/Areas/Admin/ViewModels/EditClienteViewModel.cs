using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Areas.Admin.ViewModels;

public class EditClienteViewModel
{
    [Required(ErrorMessage = "El ID es requerido")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    [Display(Name = "Nombre")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "El nombre solo puede contener letras")]
    public string Nombre { get; set; } = null!;
    
    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
    [Display(Name = "Apellido")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "El apellido solo puede contener letras")]
    public string Apellido { get; set; } = null!;
    
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    [StringLength(150, ErrorMessage = "El email no puede exceder los 150 caracteres")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;
    
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
    [Phone(ErrorMessage = "El teléfono no es válido")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }
    
    [StringLength(20, ErrorMessage = "El documento no puede exceder los 20 caracteres")]
    [Display(Name = "Documento")]
    [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "El documento contiene caracteres inválidos")]
    public string? Documento { get; set; }
    
    [Required(ErrorMessage = "La dirección es requerida")]
    [StringLength(250, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 250 caracteres")]
    [Display(Name = "Dirección")]
    public string Direccion { get; set; } = null!;
    
    [StringLength(100, ErrorMessage = "La ciudad no puede exceder los 100 caracteres")]
    [Display(Name = "Ciudad")]
    public string? Ciudad { get; set; }
    
    [StringLength(100, ErrorMessage = "El país no puede exceder los 100 caracteres")]
    [Display(Name = "País")]
    public string? Pais { get; set; }
    
    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;
}

