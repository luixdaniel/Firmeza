using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Firmeza.Web.Data;
using Firmeza.Web.Identity;

// Script para actualizar el rol "Administrador" a "Admin"

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Buscar el rol "Administrador"
        var adminRole = await roleManager.FindByNameAsync("Administrador");
        
        if (adminRole != null)
        {
            Console.WriteLine($"✅ Rol encontrado: {adminRole.Name}");
            
            // Actualizar el nombre del rol
            adminRole.Name = "Admin";
            var result = await roleManager.UpdateAsync(adminRole);
            
            if (result.Succeeded)
            {
                Console.WriteLine("✅ Rol actualizado correctamente de 'Administrador' a 'Admin'");
            }
            else
            {
                Console.WriteLine($"❌ Error al actualizar rol: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            Console.WriteLine("⚠️ No se encontró el rol 'Administrador'. Verificando si ya existe 'Admin'...");
            
            var adminRoleCheck = await roleManager.FindByNameAsync("Admin");
            if (adminRoleCheck != null)
            {
                Console.WriteLine("✅ El rol 'Admin' ya existe en la base de datos");
            }
            else
            {
                Console.WriteLine("❌ No se encontró ningún rol de administrador");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error: {ex.Message}");
    }
}

Console.WriteLine("\nPresione cualquier tecla para salir...");
Console.ReadKey();

