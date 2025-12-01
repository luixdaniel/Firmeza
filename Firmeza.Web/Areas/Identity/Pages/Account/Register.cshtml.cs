using System.ComponentModel.DataAnnotations;
using Firmeza.Web.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Firmeza.Web.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterModel(UserManager<ApplicationUser> userManager,
                         SignInManager<ApplicationUser> signInManager,
                         RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Documento")]
        public string? Documento { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Display(Name = "Ciudad")]
        public string? Ciudad { get; set; }

        [Display(Name = "País")]
        public string? Pais { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = new ApplicationUser 
        { 
            UserName = Input.Email, 
            Email = Input.Email, 
            NombreCompleto = $"{Input.Nombre} {Input.Apellido}"
        };
        
        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            // Asegurar rol Cliente existe
            if (!await _roleManager.RoleExistsAsync("Cliente"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Cliente"));
            }
            await _userManager.AddToRoleAsync(user, "Cliente");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return Page();
    }
}

