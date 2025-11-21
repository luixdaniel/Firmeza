using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Firmeza.Web.Identity;

namespace Firmeza.Web.Data.Seed
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // Roles consistentes con la API
            string[] roles = { "Admin", "Cliente" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Usuario administrador por defecto (mismas credenciales que la API)
            var adminEmail = "admin@firmeza.com";
            var adminPass = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Nombre = "Admin",
                    Apellido = "Sistema",
                    NombreCompleto = "Admin Sistema"
                };

                var result = await userManager.CreateAsync(admin, adminPass);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}