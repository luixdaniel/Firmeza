using Microsoft.AspNetCore.Identity;

namespace Firmeza.Web.Identity;

public class ApplicationUser : IdentityUser
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    
    public string? NombreCompleto { get; set; }
}
