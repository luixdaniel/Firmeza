# ✅ Correcciones Aplicadas - Conexión PostgreSQL/Supabase

## 🔧 Cambios Realizados

### 1. **ApiFirmeza.Web.csproj**
❌ **Antes:** Usaba `Microsoft.EntityFrameworkCore.SqlServer`  
✅ **Ahora:** Usa `Npgsql.EntityFrameworkCore.PostgreSQL`

```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.0"/>
```

### 2. **Program.cs**
❌ **Antes:** `options.UseSqlServer(connectionString)`  
✅ **Ahora:** `options.UseNpgsql(connectionString)`

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
```

### 3. **Connection String**
✅ Ya está configurada correctamente en `secrets.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.qqvyetzzgyxaauedovkv;Password=luis1206;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

---

## 🚀 Cómo Ejecutar Ahora

### Opción 1: Script (Recomendado)
```bash
# Haz doble clic en:
run-api.bat
```

### Opción 2: Desde Rider
1. Para cualquier ejecución anterior (si está corriendo)
2. Click derecho en **ApiFirmeza.Web**
3. **Run** o presiona **F5**

### Opción 3: Terminal
```bash
cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
dotnet run
```

---

## 🧪 Probar la Conexión

Una vez iniciada la API, abre Swagger:
```
https://localhost:5001
```

Prueba este endpoint primero:
```
GET /api/categorias
```

**Respuestas posibles:**

### ✅ Éxito (200 OK):
```json
[]  // Lista vacía (normal si no hay datos)
```
o
```json
[
  {
    "id": 1,
    "nombre": "Electrónica",
    ...
  }
]
```

### ❌ Error 500:
Verifica en la **terminal** el mensaje de error específico.

---

## 🔍 Si Aún Da Error 500

### Posibles Causas:

#### 1. **Tablas no existen en Supabase**
**Solución:** Ejecutar migraciones
```bash
cd C:\Users\luisc\RiderProjects\Firmeza\Firmeza.Web
dotnet ef database update
```

#### 2. **Credenciales incorrectas**
Verifica en Supabase:
- Host
- Puerto (5432)
- Usuario
- Contraseña

#### 3. **Firewall/SSL**
Si falla por SSL, modifica el connection string:
```json
"DefaultConnection": "Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.qqvyetzzgyxaauedovkv;Password=luis1206;SSL Mode=Prefer"
```

#### 4. **Modelo no coincide con BD**
Verifica que las tablas en Supabase tengan las mismas columnas que tus entidades.

---

## 📊 Verificar en Supabase

1. Ve a tu proyecto en Supabase
2. Click en **Table Editor**
3. Verifica que existan estas tablas:
   - `Categorias`
   - `Productos`
   - `Clientes`
   - `Ventas`
   - `DetallesDeVenta`

---

## 🎯 Siguiente Paso

**Para la API y reiníciala:**
1. Presiona `Ctrl + C` en la terminal donde corre
2. Ejecuta nuevamente: `dotnet run`
3. Abre Swagger: `https://localhost:5001`
4. Prueba `GET /api/categorias`

---

## 📝 Resumen de Archivos Modificados

```
✅ ApiFirmeza.Web.csproj     - Cambiado a Npgsql
✅ Program.cs                - Cambiado a UseNpgsql
✅ run-api.bat               - Script actualizado
✅ secrets.json              - Ya estaba correcto
```

---

## 💡 Comandos Útiles

```bash
# Ver logs en tiempo real
dotnet run --verbosity detailed

# Verificar conexión string
dotnet user-secrets list --project ApiFirmeza.Web

# Crear/actualizar base de datos
cd Firmeza.Web
dotnet ef database update

# Crear nueva migración
dotnet ef migrations add NombreMigracion
```

---

## 🆘 Compartir Error

Si sigue dando error 500, comparte:

1. **El mensaje en la terminal** donde corre `dotnet run`
2. **El mensaje en Swagger** (Response body)
3. Yo te ayudaré a solucionarlo específicamente

---

¡Ahora tu API debería conectar correctamente a Supabase! 🎉

