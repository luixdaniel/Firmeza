#!/bin/bash

echo "==================================="
echo "🧪 TEST: FLUJO COMPLETO CLIENTE"
echo "==================================="
echo ""

API_URL="http://localhost:5090/api"
EMAIL="testcliente@test.com"
PASSWORD="Cliente123\$"

echo "📝 PASO 1: Login del cliente"
echo "Email: $EMAIL"
echo ""

LOGIN_RESPONSE=$(curl -s -X POST "$API_URL/Auth/login" \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"$EMAIL\",\"password\":\"$PASSWORD\"}")

echo "Respuesta del login:"
echo "$LOGIN_RESPONSE" | jq . 2>/dev/null || echo "$LOGIN_RESPONSE"
echo ""

TOKEN=$(echo "$LOGIN_RESPONSE" | jq -r '.token' 2>/dev/null)

if [ "$TOKEN" == "null" ] || [ -z "$TOKEN" ]; then
    echo "❌ ERROR: No se pudo obtener el token"
    exit 1
fi

echo "✅ Token obtenido exitosamente"
echo "Token: ${TOKEN:0:50}..."
echo ""

echo "-----------------------------------"
echo "📝 PASO 2: Obtener perfil del cliente"
echo ""

PERFIL_RESPONSE=$(curl -s -X GET "$API_URL/Clientes/perfil" \
  -H "Authorization: Bearer $TOKEN")

echo "Respuesta del perfil:"
echo "$PERFIL_RESPONSE" | jq . 2>/dev/null || echo "$PERFIL_RESPONSE"
echo ""

CLIENTE_ID=$(echo "$PERFIL_RESPONSE" | jq -r '.id' 2>/dev/null)

if [ "$CLIENTE_ID" == "null" ] || [ -z "$CLIENTE_ID" ]; then
    echo "❌ ERROR: No se pudo obtener el ID del cliente"
    exit 1
fi

echo "✅ Cliente ID: $CLIENTE_ID"
echo ""

echo "-----------------------------------"
echo "📝 PASO 3: Obtener productos disponibles"
echo ""

PRODUCTOS_RESPONSE=$(curl -s -X GET "$API_URL/Productos" \
  -H "Authorization: Bearer $TOKEN")

echo "Productos disponibles:"
echo "$PRODUCTOS_RESPONSE" | jq '. | length' 2>/dev/null
PRIMER_PRODUCTO=$(echo "$PRODUCTOS_RESPONSE" | jq '.[0]' 2>/dev/null)
echo "Primer producto:"
echo "$PRIMER_PRODUCTO" | jq . 2>/dev/null
echo ""

PRODUCTO_ID=$(echo "$PRIMER_PRODUCTO" | jq -r '.id' 2>/dev/null)
PRODUCTO_PRECIO=$(echo "$PRIMER_PRODUCTO" | jq -r '.precio' 2>/dev/null)

echo "-----------------------------------"
echo "📝 PASO 4: Crear una venta (compra)"
echo ""

VENTA_DATA="{
  \"metodoPago\": \"Efectivo\",
  \"detalles\": [
    {
      \"productoId\": $PRODUCTO_ID,
      \"cantidad\": 2,
      \"precioUnitario\": $PRODUCTO_PRECIO
    }
  ]
}"

echo "Datos de la venta:"
echo "$VENTA_DATA" | jq . 2>/dev/null
echo ""

VENTA_RESPONSE=$(curl -s -X POST "$API_URL/Ventas" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "$VENTA_DATA")

echo "Respuesta de la venta:"
echo "$VENTA_RESPONSE" | jq . 2>/dev/null || echo "$VENTA_RESPONSE"
echo ""

VENTA_ID=$(echo "$VENTA_RESPONSE" | jq -r '.id' 2>/dev/null)

if [ "$VENTA_ID" == "null" ] || [ -z "$VENTA_ID" ]; then
    echo "❌ ERROR: No se pudo crear la venta"
    exit 1
fi

echo "✅ Venta creada exitosamente con ID: $VENTA_ID"
echo ""

echo "-----------------------------------"
echo "📝 PASO 5: Verificar 'Mis Compras'"
echo ""

sleep 2  # Esperar un momento para que se procese

MIS_COMPRAS_RESPONSE=$(curl -s -X GET "$API_URL/Ventas/mis-compras" \
  -H "Authorization: Bearer $TOKEN")

echo "Mis Compras:"
echo "$MIS_COMPRAS_RESPONSE" | jq . 2>/dev/null || echo "$MIS_COMPRAS_RESPONSE"
echo ""

CANTIDAD_COMPRAS=$(echo "$MIS_COMPRAS_RESPONSE" | jq '. | length' 2>/dev/null)

echo "-----------------------------------"
echo "📊 RESUMEN"
echo "-----------------------------------"
echo "Cliente ID: $CLIENTE_ID"
echo "Venta creada ID: $VENTA_ID"
echo "Total de compras: $CANTIDAD_COMPRAS"
echo ""

if [ "$CANTIDAD_COMPRAS" -gt 0 ]; then
    echo "✅ ¡TEST EXITOSO! El cliente tiene compras registradas"
else
    echo "❌ ERROR: El cliente no tiene compras registradas"
    echo "⚠️  Revisa los logs de la API para más información"
fi

echo "==================================="

