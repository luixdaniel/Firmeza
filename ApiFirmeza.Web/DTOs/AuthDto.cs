using System.ComponentModel.DataAnnotations;

namespace ApiFirmeza.Web.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Debe confirmar la contraseña")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;
    
    [Phone(ErrorMessage = "El teléfono no es válido")]
    public string? Telefono { get; set; }
}

public class LoginDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La contraseña es requerida")]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string Email { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

