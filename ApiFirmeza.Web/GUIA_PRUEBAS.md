# 🧪 Guía de Pruebas - Firmeza API

## 🚀 Cómo ejecutar la API

### Opción 1: Desde la terminal
```bash
cd C:\Users\luisc\RiderProjects\Firmeza\ApiFirmeza.Web
dotnet run
```

### Opción 2: Desde Visual Studio / Rider
- Abrir el proyecto en el IDE
- Establecer ApiFirmeza.Web como proyecto de inicio
- Presionar F5 o hacer clic en "Run"

### Puerto por defecto
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

---

## 📚 Acceder a Swagger UI

Una vez que la API esté ejecutándose:

1. Abrir el navegador
2. Ir a: `https://localhost:5001` o `http://localhost:5000`
3. Verás la interfaz de Swagger con todos los endpoints documentados

---

## 🧪 Pruebas con Swagger

### 1️⃣ Crear una Categoría

**Endpoint:** `POST /api/categorias`

**Body:**
```json
{
  "nombre": "Electrónica",
  "descripcion": "Productos electrónicos y tecnología"
}
```

**Respuesta esperada:** Status 201 Created

---

### 2️⃣ Listar Categorías

**Endpoint:** `GET /api/categorias`

**Respuesta esperada:** 
```json
[
  {
    "id": 1,
    "nombre": "Electrónica",
    "descripcion": "Productos electrónicos y tecnología",
    "cantidadProductos": 0
  }
]
```

---

### 3️⃣ Crear un Producto

**Endpoint:** `POST /api/productos`

**Body:**
```json
{
  "nombre": "Laptop Dell XPS 15",
  "descripcion": "Laptop de alto rendimiento con procesador Intel i7",
  "precio": 1299.99,
  "stock": 10,
  "categoriaId": 1
}
```

**Respuesta esperada:** Status 201 Created

---

### 4️⃣ Listar Productos

**Endpoint:** `GET /api/productos`

**Respuesta esperada:** Lista con el producto creado incluyendo nombre de categoría

---

### 5️⃣ Buscar Productos

**Endpoint:** `GET /api/productos/buscar?termino=laptop`

**Respuesta esperada:** Lista de productos que coincidan con "laptop"

---

### 6️⃣ Crear un Cliente

**Endpoint:** `POST /api/clientes`

**Body:**
```json
{
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan.perez@email.com",
  "telefono": "555-1234",
  "documento": "12345678",
  "direccion": "Calle Principal 123",
  "ciudad": "Ciudad de México",
  "pais": "México"
}
```

**Respuesta esperada:** Status 201 Created

---

### 7️⃣ Crear una Venta

**Endpoint:** `POST /api/ventas`

**Body:**
```json
{
  "cliente": "Juan Pérez",
  "clienteId": 1,
  "metodoPago": "Tarjeta",
  "vendedor": "Admin",
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2,
      "precioUnitario": 1299.99
    }
  ]
}
```

**Nota:** 
- El sistema calcula automáticamente: Subtotal, IVA (19%) y Total
- El stock del producto se actualiza automáticamente
- Se genera un número de factura único

**Respuesta esperada:** Status 201 Created con todos los detalles de la venta

---

### 8️⃣ Listar Ventas

**Endpoint:** `GET /api/ventas`

**Respuesta esperada:** Lista de todas las ventas con sus detalles

---

### 9️⃣ Obtener Ventas de un Cliente

**Endpoint:** `GET /api/ventas/cliente/1`

**Respuesta esperada:** Lista de ventas del cliente con ID 1

---

### 🔟 Actualizar un Producto

**Endpoint:** `PUT /api/productos/1`

**Body:**
```json
{
  "nombre": "Laptop Dell XPS 15 (Actualizada)",
  "descripcion": "Laptop de alto rendimiento con procesador Intel i7 y 16GB RAM",
  "precio": 1399.99,
  "stock": 8,
  "categoriaId": 1
}
```

**Respuesta esperada:** Status 204 No Content

---

### 1️⃣1️⃣ Eliminar un Producto

**Endpoint:** `DELETE /api/productos/1`

**Respuesta esperada:** Status 204 No Content

**Nota:** No se puede eliminar si tiene ventas asociadas

---

## 🧪 Pruebas con cURL

### Crear una categoría
```bash
curl -X POST "https://localhost:5001/api/categorias" ^
  -H "Content-Type: application/json" ^
  -d "{\"nombre\":\"Electrónica\",\"descripcion\":\"Productos electrónicos\"}"
```

### Obtener todas las categorías
```bash
curl -X GET "https://localhost:5001/api/categorias"
```

### Crear un producto
```bash
curl -X POST "https://localhost:5001/api/productos" ^
  -H "Content-Type: application/json" ^
  -d "{\"nombre\":\"Laptop\",\"descripcion\":\"Laptop Dell\",\"precio\":1299.99,\"stock\":10,\"categoriaId\":1}"
```

---

## 🧪 Pruebas con Postman

1. **Importar endpoints:**
   - Abrir Postman
   - Importar desde URL: `https://localhost:5001/swagger/v1/swagger.json`

2. **Configurar variables:**
   - Base URL: `https://localhost:5001`
   
3. **Ejecutar las peticiones en orden:**
   - Categorías → Productos → Clientes → Ventas

---

## ✅ Validaciones a Probar

### Producto
- ❌ Crear producto sin categoría válida → Error 400
- ❌ Crear producto con precio negativo → Error 400
- ❌ Crear producto con stock negativo → Error 400

### Cliente
- ❌ Crear cliente sin email → Error 400
- ❌ Crear cliente con email inválido → Error 400
- ❌ Crear cliente sin nombre → Error 400

### Venta
- ❌ Crear venta sin detalles → Error 400
- ❌ Crear venta con producto inexistente → Error 400
- ❌ Crear venta con stock insuficiente → Error 400
- ✅ Stock se actualiza después de crear venta

### Categoría
- ❌ Eliminar categoría con productos asociados → Error 400

---

## 🔍 Health Check

Verificar que la API está funcionando:

**Endpoint:** `GET /health`

**Respuesta esperada:**
```json
{
  "status": "Healthy",
  "timestamp": "2025-01-18T10:30:00.000Z",
  "environment": "Development"
}
```

---

## 📊 Verificar Base de Datos

Después de crear datos, puedes verificar en SQL Server:

```sql
-- Ver categorías
SELECT * FROM Categorias;

-- Ver productos
SELECT * FROM Productos;

-- Ver clientes
SELECT * FROM Clientes;

-- Ver ventas con detalles
SELECT v.*, d.* 
FROM Ventas v
INNER JOIN DetallesDeVenta d ON v.Id = d.VentaId;

-- Verificar stock actualizado
SELECT Id, Nombre, Stock FROM Productos;
```

---

## 🐛 Solución de Problemas

### Error: "Cannot connect to database"
- Verificar que SQL Server está ejecutándose
- Verificar la connection string en `appsettings.json`
- Ejecutar las migraciones del proyecto Firmeza.Web

### Error: "CORS policy"
- Ya está configurado para permitir todos los orígenes en desarrollo
- Si persiste, revisar la configuración en `Program.cs`

### Error: "Port already in use"
- Cambiar el puerto en `Properties/launchSettings.json`
- O detener la otra aplicación que esté usando el puerto

### Error 404 en endpoints
- Verificar que la ruta es correcta: `/api/[controlador]`
- Ejemplo: `/api/productos`, no `/productos`

---

## 📝 Notas Importantes

1. **Base de datos compartida:** Esta API usa la misma base de datos que Firmeza.Web
2. **Sin autenticación:** Actualmente no hay autenticación implementada
3. **CORS abierto:** Solo para desarrollo, restringir en producción
4. **Logs:** Los logs se muestran en la consola al ejecutar la API
5. **Swagger solo en Development:** En producción está deshabilitado

---

## 🎯 Flujo Completo de Prueba

1. ✅ Crear 2-3 categorías
2. ✅ Crear 5-10 productos en diferentes categorías
3. ✅ Crear 3-5 clientes
4. ✅ Crear 2-3 ventas con múltiples productos
5. ✅ Verificar que el stock se actualiza
6. ✅ Buscar productos por nombre
7. ✅ Obtener ventas de un cliente específico
8. ✅ Actualizar un producto
9. ✅ Intentar eliminar una categoría con productos (debe fallar)
10. ✅ Verificar el health check

---

## 📚 Próximos Pasos

- [ ] Implementar autenticación JWT
- [ ] Agregar paginación a los listados
- [ ] Implementar filtros avanzados
- [ ] Agregar endpoints de reportes
- [ ] Implementar caché con Redis
- [ ] Agregar rate limiting
- [ ] Implementar versionado de API
- [ ] Agregar tests unitarios e integración

