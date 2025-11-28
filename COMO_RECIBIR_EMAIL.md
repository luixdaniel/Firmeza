# 🎯 INSTRUCCIONES FINALES - Envío de Email con tu Usuario

## ✅ Tu Usuario ya está Creado

```
Email: muyguapoluisguapo@gmail.com
Contraseña: Luis1206$
```

## 📧 Cómo Recibir el Comprobante en tu Email

### OPCIÓN 1: Desde el Frontend (MÁS FÁCIL) ⭐

1. **Inicia la API** (si no está corriendo):
```bash
cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web
dotnet run
```

2. **Inicia el Frontend** (en otra terminal):
```bash
cd /home/Coder/Escritorio/Firmeza/firmeza-client
npm run dev
```

3. **Abre tu navegador**:
   - Ve a: http://localhost:3000

4. **Haz Login**:
   - Email: `muyguapoluisguapo@gmail.com`
   - Contraseña: `Luis1206$`

5. **Realiza una Compra**:
   - Ve a la tienda
   - Agrega productos al carrito
   - Haz clic en "Procesar Compra"
   - Verás el mensaje: **"Compra realizada exitosamente. El comprobante será enviado a tu correo electrónico."**

6. **Revisa tu Email**:
   - Ve a: https://mail.google.com
   - Login con: muyguapoluisguapo@gmail.com
   - Busca email de: **ceraluis4@gmail.com**
   - Asunto: **"Comprobante de Compra - Factura [CODIGO]"**
   - Si no lo ves, revisa **SPAM** o **Promociones**

---

### OPCIÓN 2: Desde la API (Manual)

Si prefieres probar directamente con la API:

```bash
# 1. Asegúrate que la API esté corriendo
curl http://localhost:5090/health

# 2. Ejecuta este script
cd /home/Coder/Escritorio/Firmeza
bash << 'SCRIPT'
API_URL="http://localhost:5090/api"

# Login
TOKEN=$(curl -s -X POST "$API_URL/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"muyguapoluisguapo@gmail.com","password":"Luis1206$"}' | jq -r '.token')

# Obtener producto
PRODUCTO=$(curl -s -X GET "$API_URL/Productos" -H "Authorization: Bearer $TOKEN" | jq '.[0]')
PRODUCTO_ID=$(echo "$PRODUCTO" | jq -r '.id')
PRODUCTO_PRECIO=$(echo "$PRODUCTO" | jq -r '.precio')

# Crear venta
curl -s -X POST "$API_URL/Ventas" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "{
    \"metodoPago\": \"Tarjeta\",
    \"detalles\": [{
      \"productoId\": $PRODUCTO_ID,
      \"cantidad\": 1,
      \"precioUnitario\": $PRODUCTO_PRECIO
    }]
  }" | jq '.'

echo ""
echo "✅ Compra realizada!"
echo "📧 Revisa tu email: muyguapoluisguapo@gmail.com"
SCRIPT
```

---

## 📧 Qué Esperar en el Email

### Remitente:
- **Nombre**: Firmeza - Tienda Online
- **Email**: ceraluis4@gmail.com

### Asunto:
```
Comprobante de Compra - Factura [CODIGO-ALEATORIO]
```

### Contenido:
1. **Email HTML bonito** con:
   - Encabezado morado con gradiente
   - Mensaje: "¡Gracias por tu compra!"
   - Detalles de la compra
   - Total pagado destacado

2. **PDF Adjunto** con:
   - Logo Firmeza
   - Número de factura
   - Tus datos
   - Lista de productos comprados
   - Precios, subtotal, IVA, total

---

## ⏱️ Tiempo de Espera

- El email se envía **inmediatamente** después de la compra
- Puede tardar **1-3 minutos** en llegar a tu bandeja
- Si no llega en 5 minutos, revisa:
  1. **Carpeta SPAM**
  2. **Carpeta Promociones** (Gmail)
  3. **Carpeta Social** (Gmail)
  4. Logs de la API para ver si hubo errores

---

## 🔍 Verificar Logs de la API

Si quieres ver si el email se envió, busca estos mensajes en la consola donde corre `dotnet run`:

```
✅ Create Venta - Venta creada exitosamente: VentaId=XX
📧 Iniciando envío de comprobante por email para Venta ID: XX
📧 Enviando comprobante de compra a muyguapoluisguapo@gmail.com
📎 PDF adjunto: XXXX bytes
✅ Correo enviado exitosamente a muyguapoluisguapo@gmail.com
```

Si ves:
```
❌ Error al enviar correo a muyguapoluisguapo@gmail.com: ...
```

Entonces hay un problema con las credenciales de Gmail o la conexión SMTP.

---

## 🎯 RESUMEN

1. ✅ Tu usuario **muyguapoluisguapo@gmail.com** ya existe
2. ✅ La funcionalidad de envío de emails está implementada
3. ✅ Las credenciales de Gmail están configuradas
4. ✅ Solo necesitas hacer una compra desde el frontend
5. ✅ El email llegará a tu buzón automáticamente

**¡Pruébalo ahora mismo!** 🚀

