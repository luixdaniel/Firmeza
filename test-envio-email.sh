#!/bin/bash

echo "=========================================="
echo "📧 TEST: Envío de Comprobante por Email"
echo "=========================================="
echo ""

API_URL="http://localhost:5090/api"

# Colores
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Verificar que la API esté corriendo
echo "🔍 Verificando que la API esté corriendo..."
if ! curl -s http://localhost:5090/health > /dev/null 2>&1; then
    echo -e "${RED}❌ Error: La API no está corriendo en el puerto 5090${NC}"
    echo "Inicia la API con: cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web && dotnet run"
    exit 1
fi
echo -e "${GREEN}✅ API corriendo correctamente${NC}"
echo ""

# Login
echo "1️⃣ LOGIN..."
TOKEN=$(curl -s -X POST "$API_URL/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"testcliente@test.com","password":"Cliente123$"}' | jq -r '.token')

if [ "$TOKEN" == "null" ] || [ -z "$TOKEN" ]; then
    echo -e "${RED}❌ Error en login${NC}"
    exit 1
fi
echo -e "${GREEN}✅ Token obtenido${NC}"
echo ""

# Obtener perfil
echo "2️⃣ OBTENIENDO PERFIL DEL CLIENTE..."
PERFIL=$(curl -s -X GET "$API_URL/Clientes/perfil" -H "Authorization: Bearer $TOKEN")
CLIENTE_EMAIL=$(echo "$PERFIL" | jq -r '.email')
CLIENTE_NOMBRE=$(echo "$PERFIL" | jq -r '.nombreCompleto')

echo "Email: $CLIENTE_EMAIL"
echo "Nombre: $CLIENTE_NOMBRE"
echo ""

# Verificar configuración de email
echo "3️⃣ VERIFICANDO CONFIGURACIÓN DE EMAIL..."
echo -e "${YELLOW}⚠️  IMPORTANTE: Asegúrate de haber configurado las credenciales de Gmail en appsettings.json${NC}"
echo ""
echo "El email del comprobante se enviará a: ${GREEN}$CLIENTE_EMAIL${NC}"
echo ""
read -p "¿Deseas continuar con la prueba? (s/n): " -n 1 -r
echo ""
if [[ ! $REPLY =~ ^[Ss]$ ]]; then
    echo "Prueba cancelada."
    exit 0
fi

# Obtener productos
echo ""
echo "4️⃣ OBTENIENDO PRODUCTOS..."
PRODUCTOS=$(curl -s -X GET "$API_URL/Productos" -H "Authorization: Bearer $TOKEN")
PRODUCTO_ID=$(echo "$PRODUCTOS" | jq -r '.[0].id')
PRODUCTO_PRECIO=$(echo "$PRODUCTOS" | jq -r '.[0].precio')
PRODUCTO_NOMBRE=$(echo "$PRODUCTOS" | jq -r '.[0].nombre')

echo "Producto: $PRODUCTO_NOMBRE (ID: $PRODUCTO_ID)"
echo "Precio: \$$PRODUCTO_PRECIO"
echo ""

# Crear venta
echo "5️⃣ CREANDO VENTA Y ENVIANDO COMPROBANTE..."
VENTA_REQUEST="{
  \"metodoPago\": \"Efectivo\",
  \"detalles\": [
    {
      \"productoId\": $PRODUCTO_ID,
      \"cantidad\": 1,
      \"precioUnitario\": $PRODUCTO_PRECIO
    }
  ]
}"

VENTA_RESPONSE=$(curl -s -X POST "$API_URL/Ventas" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "$VENTA_REQUEST")

echo "Respuesta de la API:"
echo "$VENTA_RESPONSE" | jq .
echo ""

# Verificar si hay venta
VENTA_ID=$(echo "$VENTA_RESPONSE" | jq -r '.venta.id // .id')
MENSAJE=$(echo "$VENTA_RESPONSE" | jq -r '.mensaje // empty')

if [ "$VENTA_ID" == "null" ] || [ -z "$VENTA_ID" ]; then
    echo -e "${RED}❌ Error al crear venta${NC}"
    echo "Detalles: $(echo "$VENTA_RESPONSE" | jq -r '.message // .error // "Error desconocido"')"
    exit 1
fi

echo -e "${GREEN}✅ Venta creada exitosamente con ID: $VENTA_ID${NC}"
if [ ! -z "$MENSAJE" ]; then
    echo -e "${GREEN}📧 $MENSAJE${NC}"
fi
echo ""

# Esperar un poco para que se procese el email
echo "⏳ Esperando 5 segundos para que se procese el envío del email..."
sleep 5
echo ""

# Verificar en mis-compras
echo "6️⃣ VERIFICANDO EN 'MIS COMPRAS'..."
MIS_COMPRAS=$(curl -s -X GET "$API_URL/Ventas/mis-compras" \
  -H "Authorization: Bearer $TOKEN")

CANTIDAD=$(echo "$MIS_COMPRAS" | jq 'length')

echo "Total de compras: $CANTIDAD"
echo ""

# Resumen final
echo "=========================================="
echo "✅ RESUMEN FINAL"
echo "=========================================="
echo -e "Cliente: ${GREEN}$CLIENTE_NOMBRE${NC}"
echo -e "Email: ${GREEN}$CLIENTE_EMAIL${NC}"
echo -e "Venta ID: ${GREEN}$VENTA_ID${NC}"
echo -e "Total de compras: ${GREEN}$CANTIDAD${NC}"
echo ""
echo "📧 VERIFICACIÓN DE EMAIL:"
echo "----------------------------------------"
echo "1. Revisa tu bandeja de entrada: $CLIENTE_EMAIL"
echo "2. Si no lo encuentras, revisa la carpeta de SPAM"
echo "3. Busca el asunto: 'Comprobante de Compra - Factura [NUMERO]'"
echo "4. El email debe contener un PDF adjunto"
echo ""
echo "🔍 VERIFICACIÓN DE LOGS:"
echo "----------------------------------------"
echo "En la consola de la API deberías ver:"
echo "  📧 Enviando comprobante de compra a $CLIENTE_EMAIL"
echo "  📎 PDF adjunto: [tamaño] bytes"
echo "  ✅ Correo enviado exitosamente a $CLIENTE_EMAIL"
echo ""

if [ "$CANTIDAD" -gt 0 ]; then
    echo -e "${GREEN}🎉 ¡ÉXITO! La compra se registró correctamente${NC}"
else
    echo -e "${RED}⚠️  Advertencia: No se encontraron compras${NC}"
fi

echo "=========================================="

