using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Data.Entities;

public class Cliente
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres")]
    public string Apellido { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(20)]
    [Phone(ErrorMessage = "El teléfono no es válido")]
    public string? Telefono { get; set; }
    
    [StringLength(20)]
    public string? Documento { get; set; }
    
    [Required]
    [StringLength(250)]
    public string Direccion { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Ciudad { get; set; }
    
    [StringLength(100)]
    public string? Pais { get; set; }
    
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    
    public bool Activo { get; set; } = true;
    
    // Propiedad calculada para nombre completo
    public string NombreCompleto => $"{Nombre} {Apellido}";
    
    // Relación: Un cliente puede tener muchas ventas
    public ICollection<Venta>? Ventas { get; set; }
}