using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Areas.Admin.ViewModels;

/// <summary>
/// ViewModel para mostrar información de clientes (Index, Details)
/// </summary>
public class ClienteViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = null!;
    
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = null!;
    
    [Display(Name = "Nombre Completo")]
    public string NombreCompleto => $"{Nombre} {Apellido}";
    
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;
    
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }
    
    [Display(Name = "Documento")]
    public string? Documento { get; set; }
    
    [Display(Name = "Dirección")]
    public string Direccion { get; set; } = null!;
    
    [Display(Name = "Ciudad")]
    public string? Ciudad { get; set; }
    
    [Display(Name = "País")]
    public string? Pais { get; set; }
    
    [Display(Name = "Fecha de Registro")]
    [DataType(DataType.Date)]
    public DateTime FechaRegistro { get; set; }
    
    [Display(Name = "Activo")]
    public bool Activo { get; set; }
}

