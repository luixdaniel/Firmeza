# API Corriendo en Puerto 5090

## ✅ Estado Actual

- **API:** http://localhost:5090
- **Swagger:** http://localhost:5090/swagger
- **Frontend configurado:** Puerto 5090

## Cambios Realizados

### 1. Puerto de la API
- ✅ API iniciada en puerto **5090** (antes 5000)
- ✅ Frontend actualizado para apuntar a puerto 5090

**Archivo actualizado:**
- `/home/Coder/Escritorio/Firmeza/firmeza-client/lib/axios.ts`

```typescript
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5090';
```

### 2. Mapping de AutoMapper Corregido
- ✅ Configurado para NO sobrescribir campos que el controlador establece
- ✅ El controlador ahora tiene control total sobre:
  - `FechaVenta`
  - `NumeroFactura`
  - `Estado`
  - `Cliente`
  - `ClienteId`
  - `MetodoPago`

## Cómo Probar el Carrito

### Paso 1: Asegúrate de tener el frontend corriendo
```bash
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev
```

### Paso 2: Inicia sesión
- Ve a: http://localhost:3000/auth/login
- Email: `ceraluis4@gmail.com`
- Contraseña: (tu contraseña)

### Paso 3: Prueba el flujo de compra
1. Ve a la tienda: http://localhost:3000/clientes/tienda
2. Agrega productos al carrito
3. Ve al carrito: http://localhost:3000/clientes/carrito
4. **Selecciona el método de pago** (Efectivo, Tarjeta, o Transferencia)
5. Haz clic en "Finalizar compra"

## Diagnóstico del Error

Si aún ves "Error interno del servidor", verifica lo siguiente:

### 1. Verificar que el cliente existe
```bash
PGPASSWORD='luis1206' psql -h aws-1-us-east-1.pooler.supabase.com -p 5432 \
  -U postgres.qqvyetzzgyxaauedovkv -d postgres \
  -c "SELECT \"Id\", \"Nombre\", \"Email\", \"Activo\" FROM \"Clientes\" WHERE \"Email\" = 'ceraluis4@gmail.com';"
```

### 2. Ver logs de la API en tiempo real
Abre una nueva terminal y ejecuta:
```bash
tail -f /tmp/api-log.txt
```

Luego intenta hacer la compra y verás el error exacto.

### 3. Probar el endpoint directamente
```bash
# Obtener token
TOKEN=$(curl -s -X POST http://localhost:5090/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"ceraluis4@gmail.com","password":"TU_CONTRASEÑA"}' \
  | python3 -c "import sys, json; print(json.load(sys.stdin)['token'])")

# Crear venta de prueba
curl -X POST http://localhost:5090/api/Ventas \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "metodoPago": "Efectivo",
    "detalles": [
      {
        "productoId": 7,
        "cantidad": 1,
        "precioUnitario": 120000
      }
    ]
  }' | jq .
```

## Posibles Causas del Error

### Causa 1: Cliente no existe en la tabla Clientes
**Solución:** El usuario debe estar registrado como cliente en la tabla `Clientes` con el mismo email que en `AspNetUsers`.

Verificar:
```sql
SELECT c.*, u."Email" 
FROM "Clientes" c 
LEFT JOIN "AspNetUsers" u ON c."Email" = u."Email"
WHERE u."Email" = 'ceraluis4@gmail.com';
```

Si no existe, crear el cliente:
```sql
INSERT INTO "Clientes" 
("Nombre", "Apellido", "Email", "Telefono", "Documento", "Direccion", "Ciudad", "Pais", "FechaRegistro", "Activo")
VALUES 
('Luis', 'Cera', 'ceraluis4@gmail.com', '1234567890', '123456', 'Calle 123', 'Ciudad', 'País', NOW(), true);
```

### Causa 2: Stock insuficiente
El producto debe tener stock disponible y estar activo.

Verificar:
```sql
SELECT "Id", "Nombre", "Stock", "Activo" 
FROM "Productos" 
WHERE "Id" = 7;
```

### Causa 3: Error en el mapping
Ya corregido en el commit actual.

## Comandos Útiles

### Reiniciar la API
```bash
pkill -f "ApiFirmeza.Web"
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run --urls "http://localhost:5090"
```

### Ver procesos corriendo
```bash
ps aux | grep -E "dotnet|ApiFirmeza" | grep -v grep
```

### Probar conectividad de la API
```bash
curl http://localhost:5090/api/Productos
```

## Estructura de Datos de Venta

El endpoint `/api/Ventas` espera:
```json
{
  "clienteId": 0,  // Opcional - se obtiene del usuario autenticado
  "metodoPago": "Efectivo",  // "Efectivo", "Tarjeta", o "Transferencia"
  "detalles": [
    {
      "productoId": 7,
      "cantidad": 1,
      "precioUnitario": 120000
    }
  ]
}
```

## Logs y Debugging

La API está configurada para mostrar errores detallados en modo Development.

Para ver más detalles, revisa:
1. La consola donde corre `dotnet run`
2. Los logs en la respuesta HTTP del error
3. La tabla de eventos en la base de datos (si existe)

---
**Última actualización:** 2025-11-27
**Puerto API:** 5090
**Estado:** ✅ API corriendo, esperando pruebas

