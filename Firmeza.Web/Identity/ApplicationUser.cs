using Microsoft.AspNetCore.Identity;

namespace Firmeza.Web.Identity;

public class ApplicationUser : IdentityUser
{
    public string? NombreCompleto { get; set; }
}
