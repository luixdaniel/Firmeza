# SOLUCIÓN FINAL - API Y CARRITO

## ✅ Cambios Completados

### 1. API Configurada en Puerto 5090
- ✅ Frontend actualizado para usar puerto 5090
- ✅ Archivo: `firmeza-client/lib/axios.ts` - API_URL = 'http://localhost:5090'

### 2. Autenticación Corregida
- ✅ Agregado `[AllowAnonymous]` a:
  - `POST /api/Auth/register` - Registro público
  - `POST /api/Auth/login` - Login público
- ✅ Rutas protegidas con `[Authorize]`:
  - `GET /api/Auth/me` - Perfil del usuario
  - Todos los endpoints de Ventas
  - Endpoints de Admin

### 3. Método de Pago en el Carrito
- ✅ Agregado selector de método de pago en la UI
- ✅ Opciones: Efectivo, Tarjeta, Transferencia
- ✅ Se envía en el request de creación de venta

### 4. Corrección del Controlador de Ventas
- ✅ Obtiene automáticamente el `ClienteId` del usuario autenticado
- ✅ Establece todos los campos requeridos:
  - `MetodoPago`
  - `NumeroFactura`  
  - `Estado`
  - `FechaVenta`
  - `Cliente` (nombre completo)
- ✅ Actualiza el stock de productos después de la compra

### 5. Productos con Campo Activo
- ✅ Agregada propiedad `Activo` a la entidad Producto
- ✅ Migración aplicada a la base de datos
- ✅ Todos los productos actualizados a `Activo = true`

## 🚀 Cómo Iniciar el Sistema

### Paso 1: Iniciar la API (Puerto 5090)
```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run --urls "http://localhost:5090"
```

**Verás este mensaje cuando esté lista:**
```
Now listening on: http://localhost:5090
Application started. Press Ctrl+C to shut down.
```

### Paso 2: Iniciar el Frontend (Puerto 3000)
En otra terminal:
```bash
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev
```

### Paso 3: Acceder a la Aplicación
- **Frontend Cliente:** http://localhost:3000
- **Swagger API:** http://localhost:5090/swagger

## 📋 Flujo de Uso del Carrito

### Para Cliente Nuevo:
1. **Registrarse:** http://localhost:3000/auth/register
   - Completa el formulario
   - Automáticamente inicia sesión

2. **Ir a la Tienda:** http://localhost:3000/clientes/tienda
   - Ver productos activos con stock
   - Hacer clic en "Agregar al carrito"

3. **Ver Carrito:** http://localhost:3000/clientes/carrito
   - Revisar productos
   - **Seleccionar método de pago** (Efectivo/Tarjeta/Transferencia)
   - Hacer clic en "Finalizar compra"

4. **Ver Compras:** http://localhost:3000/clientes/mis-compras
   - Historial de todas las compras realizadas

### Para Cliente Existente:
1. **Iniciar Sesión:** http://localhost:3000/auth/login
2. Seguir pasos 2-4 anteriores

## 🔧 Troubleshooting

### Si la API no inicia:
```bash
# 1. Matar procesos anteriores
pkill -9 -f "ApiFirmeza.Web"

# 2. Liberar el puerto 5090
lsof -ti:5090 | xargs kill -9 2>/dev/null

# 3. Iniciar de nuevo
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run --urls "http://localhost:5090"
```

### Si el carrito da error "Cliente no encontrado":
El usuario debe estar registrado mediante el endpoint `/api/Auth/register` que crea automáticamente el registro de Cliente.

**Verificar en la base de datos:**
```bash
PGPASSWORD='luis1206' psql -h aws-1-us-east-1.pooler.supabase.com -p 5432 \
  -U postgres.qqvyetzzgyxaauedovkv -d postgres \
  -c "SELECT \"Id\", \"Nombre\", \"Email\" FROM \"Clientes\" WHERE \"Email\" = 'TU_EMAIL';"
```

### Si dice "Stock insuficiente":
```bash
# Verificar stock de productos
PGPASSWORD='luis1206' psql -h aws-1-us-east-1.pooler.supabase.com -p 5432 \
  -U postgres.qqvyetzzgyxaauedovkv -d postgres \
  -c "SELECT \"Id\", \"Nombre\", \"Stock\", \"Activo\" FROM \"Productos\";"
```

## 📊 Estructura de la Petición de Venta

Cuando se hace clic en "Finalizar compra", se envía:

```json
{
  "metodoPago": "Efectivo",  // o "Tarjeta" o "Transferencia"
  "detalles": [
    {
      "productoId": 7,
      "cantidad": 1,
      "precioUnitario": 120000
    }
  ]
}
```

**Nota:** `clienteId` NO se envía, se obtiene automáticamente del token JWT del usuario autenticado.

## ✅ Validaciones Implementadas

### Backend:
- Usuario autenticado (JWT válido)
- Cliente existe en la base de datos
- Productos existen
- Stock suficiente para cada producto
- Stock se reduce automáticamente
- Cálculo automático de IVA (19%)

### Frontend:
- Token JWT presente
- Carrito no vacío
- Método de pago seleccionado
- Confirmación visual del estado

## 🎯 Endpoints Clave

### Públicos (sin autenticación):
- `POST /api/Auth/register` - Registrar cliente
- `POST /api/Auth/login` - Iniciar sesión
- `GET /api/Productos` - Listar productos
- `GET /api/Categorias` - Listar categorías

### Protegidos (requieren JWT):
- `POST /api/Ventas` - Crear venta (finalizar compra)
- `GET /api/Ventas/mis-compras` - Ver mis compras
- `GET /api/Auth/me` - Ver mi perfil

### Solo Admin:
- `GET /api/Ventas` - Ver todas las ventas
- `POST /api/Productos` - Crear producto
- `PUT /api/Productos/{id}` - Actualizar producto

## 📝 Notas Importantes

1. **Puerto 5090:** La API DEBE estar en el puerto 5090 para que el frontend funcione correctamente.

2. **Registro obligatorio:** Los usuarios deben registrarse mediante `/api/Auth/register` para que se cree su registro de Cliente automáticamente.

3. **Stock:** El stock se actualiza automáticamente al finalizar una compra y NO se puede deshacer.

4. **Métodos de pago:** Solo son informativos, el sistema no procesa pagos reales.

5. **IVA:** Se calcula automáticamente como 19% del subtotal.

## 🐛 Errores Comunes Resueltos

- ❌ "Error interno del servidor" → ✅ `ClienteId` se obtiene automáticamente
- ❌ "Productos inactivos" → ✅ Campo `Activo` agregado y configurado
- ❌ "Puerto 5000 no responde" → ✅ Cambiado a puerto 5090
- ❌ "No se pudo conectar a la API" → ✅ Rutas de Auth con `[AllowAnonymous]`
- ❌ "Falta método de pago" → ✅ Selector agregado en UI del carrito

---

**Estado Final:** ✅ SISTEMA COMPLETAMENTE FUNCIONAL
**Fecha:** 2025-11-27
**Puerto API:** 5090
**Puerto Frontend:** 3000

