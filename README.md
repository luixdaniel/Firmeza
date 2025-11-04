# Firmeza.Web

## Autenticación y roles
- Identity configurado con `ApplicationUser` y roles.
- Roles semilla: `Administrador`, `Cliente`.
- Usuario admin: `admin@firmeza.com` / `Admin123$`.
- Área Admin protegida por rol Administrador: `/Admin/Dashboard`.
- Clientes no pueden acceder a Razor (área Admin) y serán redirigidos a `/Home/AccessDenied`.

## Migraciones
- Las migraciones se aplican al arrancar con `db.Database.Migrate()`.
- Si necesitas generarlas manualmente:

```bat
cd C:\Users\luisc\RiderProjects\Firmeza\Firmeza.Web
 dotnet tool update -g dotnet-ef
 dotnet ef migrations add Initial
 dotnet ef database update
```

## Notas
- Asegura que la cadena de conexión correcta está en `appsettings.Development.json` (si ejecutas en Development) o `appsettings.json`.
- En producción/Docker (Linux), respeta mayúsculas de carpetas de Views.

