using Firmeza.Web.Data;
using Firmeza.Web.Identity;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;
using Firmeza.Web.Repositories;
using Firmeza.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(60); // 60 segundos de timeout
        npgsqlOptions.EnableRetryOnFailure(3); // 3 reintentos
    }));

// Configuración de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configuración de contraseñas
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    
    // Configuración de usuario
    options.User.RequireUniqueEmail = true;
    
    // Configuración de bloqueo
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configuración de JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role // CRÍTICO: Para que reconozca los roles en el JWT
    };
});

// Configuración de políticas de autorización
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
    .AddPolicy("ClienteOnly", policy => policy.RequireRole("Cliente"))
    .AddPolicy("AdminOrCliente", policy => policy.RequireRole("Admin", "Cliente"));

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registrar controladores
builder.Services.AddControllers();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Firmeza API",
        Version = "v1",
        Description = "API REST para el sistema de gestión Firmeza con autenticación JWT",
        Contact = new OpenApiContact
        {
            Name = "Firmeza",
            Email = "contacto@firmeza.com"
        }
    });
    
    // Configuración de seguridad JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese 'Bearer' seguido de un espacio y el token JWT.\n\nEjemplo: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Registrar repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();

// Registrar servicios
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IVentaService, VentaService>();
// PdfService comentado - no es necesario en la API
// builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IExportacionService, ExportacionService>();
builder.Services.AddScoped<IImportacionMasivaService, ImportacionMasivaService>();

// Servicios de Email y Comprobantes
builder.Services.AddScoped<ApiFirmeza.Web.Services.IEmailService, ApiFirmeza.Web.Services.EmailService>();
builder.Services.AddScoped<ApiFirmeza.Web.Services.IComprobanteService, ApiFirmeza.Web.Services.ComprobanteService>();

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configurar el pipeline HTTP
// Swagger habilitado en todos los entornos (Development y Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Firmeza API v1");
    c.RoutePrefix = "swagger"; // Swagger en /swagger
});

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Endpoint de salud
app.MapGet("/health", () => Results.Ok(new 
{ 
    status = "Healthy", 
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
})).WithTags("Health");

// Seed de roles y usuario administrador
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        
        // ⚠️ MIGRACIÓN: Actualizar rol "Administrador" a "Admin" si existe
        var oldRole = roleManager.FindByNameAsync("Administrador").GetAwaiter().GetResult();
        if (oldRole != null)
        {
            Console.WriteLine("🔄 Migrando rol 'Administrador' a 'Admin'...");
            
            // Obtener usuarios con el rol viejo
            var usersInOldRole = userManager.GetUsersInRoleAsync("Administrador").GetAwaiter().GetResult();
            
            // Crear el nuevo rol si no existe
            if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            }
            
            // Migrar usuarios al nuevo rol
            foreach (var user in usersInOldRole)
            {
                userManager.RemoveFromRoleAsync(user, "Administrador").GetAwaiter().GetResult();
                userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                Console.WriteLine($"✅ Usuario {user.Email} migrado al rol 'Admin'");
            }
            
            // Eliminar el rol viejo
            roleManager.DeleteAsync(oldRole).GetAwaiter().GetResult();
            Console.WriteLine("✅ Rol 'Administrador' eliminado");
        }
        
        // Crear roles si no existen
        string[] roles = { "Admin", "Cliente" };
        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                Console.WriteLine($"✅ Rol '{role}' creado");
            }
        }
        
        // Crear usuario administrador por defecto
        var adminEmail = "admin@firmeza.com";
        var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
        
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                Nombre = "Admin",
                Apellido = "Sistema"
            };
            
            var result = userManager.CreateAsync(adminUser, "Admin123$").GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
                Console.WriteLine("✅ Usuario administrador creado: admin@firmeza.com / Admin123$");
            }
        }
        else
        {
            // Verificar que el admin tenga el rol correcto
            var isInAdminRole = userManager.IsInRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            if (!isInAdminRole)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
                Console.WriteLine($"✅ Rol 'Admin' asignado a {adminEmail}");
            }
        }
    }
    catch (Exception ex)
    {
        var errorLogger = services.GetRequiredService<ILogger<Program>>();
        errorLogger.LogError(ex, "Error al crear roles y usuario administrador");
    }
}

// Mostrar enlaces de acceso
var appLogger = app.Services.GetRequiredService<ILogger<Program>>();
var urls = app.Urls;
appLogger.LogInformation("╔════════════════════════════════════════════════════════════════╗");
appLogger.LogInformation("║                    🚀 FIRMEZA API REST                         ║");
appLogger.LogInformation("╠════════════════════════════════════════════════════════════════╣");
appLogger.LogInformation("║  📍 API URL:     http://localhost:5090                         ║");
appLogger.LogInformation("║  📚 Swagger:     http://localhost:5090/swagger                 ║");
appLogger.LogInformation("║  💚 Health:      http://localhost:5090/health                  ║");
appLogger.LogInformation("╠════════════════════════════════════════════════════════════════╣");
appLogger.LogInformation("║  👤 Usuario Admin: admin@firmeza.com                           ║");
appLogger.LogInformation("║  🔑 Contraseña:    Admin123$                                   ║");
appLogger.LogInformation("╚════════════════════════════════════════════════════════════════╝");

app.Run();