using Firmeza.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Agregar la conexión con Supabase (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seeding de datos iniciales (solo en Development)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Aplicar migraciones pendientes
        db.Database.Migrate();
        
        // Agregar datos de ejemplo si no existen
        if (!db.Set<Firmeza.Web.Data.Entities.Categoria>().Any())
        {
            var categoriaGeneral = new Firmeza.Web.Data.Entities.Categoria
            {
                Nombre = "General",
                Descripcion = "Categoría general de productos"
            };
            db.Set<Firmeza.Web.Data.Entities.Categoria>().Add(categoriaGeneral);
            db.SaveChanges();
            
            var productoEjemplo = new Firmeza.Web.Data.Entities.Producto
            {
                Nombre = "Producto de Ejemplo",
                Descripcion = "Este es un producto de ejemplo",
                Precio = 99.99m,
                Stock = 10,
                CategoriaId = categoriaGeneral.Id
            };
            db.Set<Firmeza.Web.Data.Entities.Producto>().Add(productoEjemplo);
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();