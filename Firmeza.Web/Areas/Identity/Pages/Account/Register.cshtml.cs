using System.ComponentModel.DataAnnotations;
using Firmeza.Web.Data.Entities;
using Firmeza.Web.Identity;
using Firmeza.Web.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Firmeza.Web.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IClienteService _clienteService;

    public RegisterModel(UserManager<ApplicationUser> userManager,
                         SignInManager<ApplicationUser> signInManager,
                         RoleManager<IdentityRole> roleManager,
                         IClienteService clienteService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _clienteService = clienteService;
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

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Teléfono no válido")]
        public string? Telefono { get; set; }

        [Display(Name = "Documento (DNI/CI/Pasaporte)")]
        public string? Documento { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Display(Name = "Ciudad")]
        public string? Ciudad { get; set; }

        [Display(Name = "País")]
        public string? Pais { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
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

        // Crear el usuario en Identity
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

            try
            {
                // Crear el registro en la tabla Clientes
                var cliente = new Cliente
                {
                    Nombre = Input.Nombre,
                    Apellido = Input.Apellido,
                    Email = Input.Email,
                    Telefono = Input.Telefono,
                    Documento = Input.Documento,
                    Direccion = Input.Direccion,
                    Ciudad = Input.Ciudad,
                    Pais = Input.Pais,
                    ApplicationUserId = user.Id, // Asociar con el usuario de Identity
                    FechaRegistro = DateTime.UtcNow,
                    Activo = true
                };

                await _clienteService.CreateAsync(cliente);
                
                TempData["SuccessMessage"] = "Registro exitoso. Bienvenido a Firmeza!";
            }
            catch (Exception ex)
            {
                // Si falla la creación del cliente, registrar el error pero no bloquear el registro
                TempData["WarningMessage"] = $"Usuario creado pero hubo un problema al crear el perfil de cliente: {ex.Message}";
            }

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

