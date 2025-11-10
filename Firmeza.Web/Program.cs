using Firmeza.Web.Data;
using Firmeza.Web.Data.Seed;
using Firmeza.Web.Identity;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;
using Firmeza.Web.Repositories;
using Firmeza.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Agregar la conexión con Supabase (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'")));

// Configurar Identity con ApplicationUser y UI por defecto
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddDefaultTokenProviders()
.AddDefaultUI();

// Registrar Stores de Identity explícitamente
builder.Services.AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser, IdentityRole, AppDbContext>>();
builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole, AppDbContext>>();

// Configurar cookies (login y acceso denegado)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Home/AccessDenied";
});

// Repositorios
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// Servicios
builder.Services.AddScoped<IProductoService, ProductoService>();

// MVC y Razor Pages (Identity UI)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Migrar BD y sembrar datos al iniciar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await SeedData.InitializeAsync(services);

    // Seeding de datos de ejemplo (si deseas mantenerlo y solo en Development)
    if (app.Environment.IsDevelopment())
    {
        if (!db.Categorias.Any())
        {
            var categoriaGeneral = new Firmeza.Web.Data.Entities.Categoria
            {
                Nombre = "General",
                Descripcion = "Categoría general de productos"
            };
            db.Categorias.Add(categoriaGeneral);
            db.SaveChanges();

            var productoEjemplo = new Firmeza.Web.Data.Entities.Producto
            {
                Nombre = "Producto de Ejemplo",
                Descripcion = "Este es un producto de ejemplo",
                Precio = 99.99m,
                Stock = 10,
                CategoriaId = categoriaGeneral.Id
            };
            db.Productos.Add(productoEjemplo);
            db.SaveChanges();
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Rutas de áreas (Admin) y por defecto
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Identity UI (Razor Pages)
app.MapRazorPages();

app.Run();