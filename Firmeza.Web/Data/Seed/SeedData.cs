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

            string[] roles = { "Administrador", "Cliente" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Usuario administrador por defecto
            var adminEmail = "admin@firmeza.com";
            var adminPass = "Admin123$";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NombreCompleto = "Administrador General"
                };

                await userManager.CreateAsync(admin, adminPass);
                await userManager.AddToRoleAsync(admin, "Administrador");
            }
        }
    }
}