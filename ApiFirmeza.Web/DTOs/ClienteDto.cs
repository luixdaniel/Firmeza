using System.ComponentModel.DataAnnotations;

namespace ApiFirmeza.Web.DTOs;

public class ClienteDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Documento { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string? Ciudad { get; set; }
    public string? Pais { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool Activo { get; set; }
}

public class ClienteCreateDto
{
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
    
    [Required(ErrorMessage = "La dirección es requerida")]
    [StringLength(250)]
    public string Direccion { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Ciudad { get; set; }
    
    [StringLength(100)]
    public string? Pais { get; set; }
}

public class ClienteUpdateDto
{
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
    
    [Required(ErrorMessage = "La dirección es requerida")]
    [StringLength(250)]
    public string Direccion { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Ciudad { get; set; }
    
    [StringLength(100)]
    public string? Pais { get; set; }
    
    public bool Activo { get; set; } = true;
}

