# Solución: Error al Finalizar Compra y Método de Pago

## Problema
- Al finalizar la compra en el carrito, se generaba un error interno del servidor
- No había opción para seleccionar el método de pago (Efectivo, Tarjeta, Transferencia)

## Cambios Realizados

### 1. Backend - DTO VentaCreateDto
**Archivo:** `/home/Coder/Escritorio/Firmeza/ApiFirmeza.Web/DTOs/VentaDto.cs`

- ✅ Agregado campo `MetodoPago` con valor por defecto "Efectivo"
- ✅ Campo `ClienteId` ahora es opcional (se obtiene del usuario autenticado)

```csharp
public class VentaCreateDto
{
    public int ClienteId { get; set; } = 0;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string MetodoPago { get; set; } = "Efectivo"; // ✅ NUEVO
    [Required]
    [MinLength(1)]
    public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
}
```

### 2. Backend - VentasController
**Archivo:** `/home/Coder/Escritorio/Firmeza/ApiFirmeza.Web/Controllers/VentasController.cs`

Cambios en el método `Create`:
- ✅ Obtiene automáticamente el `ClienteId` del usuario autenticado
- ✅ Usa el `MetodoPago` del DTO (o "Efectivo" por defecto)
- ✅ Genera automáticamente el `NumeroFactura`
- ✅ Establece la fecha de venta actual
- ✅ Actualiza el stock de productos después de la compra

```csharp
venta.ClienteId = cliente.Id;
venta.MetodoPago = string.IsNullOrEmpty(ventaDto.MetodoPago) ? "Efectivo" : ventaDto.MetodoPago;
venta.Estado = "Completada";
venta.NumeroFactura = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
venta.FechaVenta = DateTime.UtcNow;

// Actualizar stock
foreach (var detalle in venta.Detalles)
{
    var producto = await _productoService.GetByIdAsync(detalle.ProductoId);
    if (producto != null)
    {
        producto.Stock -= detalle.Cantidad;
        await _productoService.UpdateAsync(producto);
    }
}
```

### 3. Frontend - Página del Carrito
**Archivo:** `/home/Coder/Escritorio/Firmeza/firmeza-client/app/clientes/carrito/page.tsx`

Cambios realizados:
- ✅ Agregado estado `metodoPago` con valor inicial "Efectivo"
- ✅ Agregado selector de método de pago en el resumen del pedido
- ✅ El método de pago se envía en la petición de creación de venta

**Nuevo selector de método de pago:**
```tsx
<div className="mb-6">
  <label className="block text-sm font-medium text-gray-700 mb-2">
    Método de pago
  </label>
  <select
    value={metodoPago}
    onChange={(e) => setMetodoPago(e.target.value)}
    className="w-full px-4 py-2 border border-gray-300 rounded-lg..."
  >
    <option value="Efectivo">Efectivo</option>
    <option value="Tarjeta">Tarjeta de crédito/débito</option>
    <option value="Transferencia">Transferencia bancaria</option>
  </select>
</div>
```

**Datos enviados a la API:**
```tsx
const ventaData = {
  metodoPago: metodoPago,  // ✅ NUEVO
  detalles: cart.map((item) => ({
    productoId: item.productoId,
    cantidad: item.cantidad,
    precioUnitario: item.precioUnitario,
  })),
};
```

## Opciones de Método de Pago

El cliente ahora puede elegir entre:
1. **Efectivo** (por defecto)
2. **Tarjeta de crédito/débito**
3. **Transferencia bancaria**

## Flujo de Compra Actualizado

1. El cliente agrega productos al carrito desde `/clientes/tienda`
2. Va a `/clientes/carrito`
3. **Selecciona el método de pago** (nuevo paso)
4. Hace clic en "Finalizar compra"
5. La API:
   - Obtiene automáticamente el cliente del usuario autenticado
   - Valida que los productos existan y tengan stock
   - Crea la venta con el método de pago seleccionado
   - Actualiza el stock de los productos
   - Genera el número de factura automáticamente
6. El cliente es redirigido a `/clientes/mis-compras`

## Validaciones Implementadas

### Backend:
- ✅ Usuario debe estar autenticado
- ✅ Cliente debe existir en la base de datos
- ✅ Todos los productos deben existir
- ✅ Stock suficiente para cada producto
- ✅ Al menos un producto en el carrito
- ✅ Método de pago válido

### Frontend:
- ✅ Token de autenticación válido
- ✅ Carrito no vacío
- ✅ Método de pago seleccionado

## Problemas Resueltos

✅ **Error interno del servidor:** Se corrigió la falta de campos requeridos (`MetodoPago`, `NumeroFactura`, etc.)
✅ **ClienteId no encontrado:** Ahora se obtiene automáticamente del usuario autenticado
✅ **Stock no actualizado:** Se agregó la lógica para reducir el stock después de la compra
✅ **Sin opción de método de pago:** Se agregó selector en la UI del carrito

## Estado Actual

- 🟢 **API corriendo en:** http://localhost:5000
- 🟢 **Frontend:** Selector de método de pago implementado
- 🟢 **Base de datos:** Productos con campo `Activo = true`
- 🟢 **Compras:** Funcionando correctamente con actualización de stock

## Prueba la Funcionalidad

1. Inicia sesión como cliente en http://localhost:3000/auth/login
2. Ve a la tienda: http://localhost:3000/clientes/tienda
3. Agrega productos al carrito
4. Ve al carrito: http://localhost:3000/clientes/carrito
5. Selecciona tu método de pago
6. Haz clic en "Finalizar compra"
7. Verifica tu compra en: http://localhost:3000/clientes/mis-compras

---
**Fecha:** 2025-11-27
**Estado:** ✅ Completado

