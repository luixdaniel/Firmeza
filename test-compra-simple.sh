#!/bin/bash

API_URL="http://localhost:5090/api"

echo "=========================================="
echo "TEST: Crear compra y verificar mis-compras"
echo "=========================================="
echo ""

# Paso 1: Login
echo "1️⃣ LOGIN..."
TOKEN=$(curl -s -X POST "$API_URL/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"testcliente@test.com","password":"Cliente123$"}' | jq -r '.token')

if [ "$TOKEN" == "null" ] || [ -z "$TOKEN" ]; then
    echo "❌ Error en login"
    exit 1
fi

echo "✅ Token obtenido"
echo ""

# Paso 2: Obtener perfil
echo "2️⃣ OBTENIENDO PERFIL..."
PERFIL=$(curl -s -X GET "$API_URL/Clientes/perfil" -H "Authorization: Bearer $TOKEN")
CLIENTE_ID=$(echo "$PERFIL" | jq -r '.id')
CLIENTE_NOMBRE=$(echo "$PERFIL" | jq -r '.nombreCompleto')

echo "Cliente ID: $CLIENTE_ID"
echo "Nombre: $CLIENTE_NOMBRE"
echo ""

# Paso 3: Obtener productos
echo "3️⃣ OBTENIENDO PRODUCTOS..."
PRODUCTOS=$(curl -s -X GET "$API_URL/Productos" -H "Authorization: Bearer $TOKEN")
PRODUCTO_ID=$(echo "$PRODUCTOS" | jq -r '.[0].id')
PRODUCTO_PRECIO=$(echo "$PRODUCTOS" | jq -r '.[0].precio')
PRODUCTO_NOMBRE=$(echo "$PRODUCTOS" | jq -r '.[0].nombre')

echo "Producto: $PRODUCTO_NOMBRE (ID: $PRODUCTO_ID)"
echo "Precio: $PRODUCTO_PRECIO"
echo ""

# Paso 4: Crear venta
echo "4️⃣ CREANDO VENTA..."
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

echo "Request:"
echo "$VENTA_REQUEST" | jq .
echo ""

VENTA_RESPONSE=$(curl -s -X POST "$API_URL/Ventas" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "$VENTA_REQUEST")

echo "Response:"
echo "$VENTA_RESPONSE" | jq .
echo ""

VENTA_ID=$(echo "$VENTA_RESPONSE" | jq -r '.id')

if [ "$VENTA_ID" == "null" ] || [ -z "$VENTA_ID" ]; then
    echo "❌ Error al crear venta"
    echo "Detalles del error:"
    echo "$VENTA_RESPONSE" | jq .
    exit 1
fi

echo "✅ Venta creada con ID: $VENTA_ID"
echo ""

# Paso 5: Esperar un momento
echo "⏳ Esperando 2 segundos..."
sleep 2
echo ""

# Paso 6: Verificar mis-compras
echo "5️⃣ VERIFICANDO MIS COMPRAS..."
MIS_COMPRAS=$(curl -s -X GET "$API_URL/Ventas/mis-compras" \
  -H "Authorization: Bearer $TOKEN")

echo "Response:"
echo "$MIS_COMPRAS" | jq .
echo ""

CANTIDAD=$(echo "$MIS_COMPRAS" | jq 'length')

echo "=========================================="
echo "RESULTADO FINAL"
echo "=========================================="
echo "Cliente ID: $CLIENTE_ID"
echo "Venta creada: $VENTA_ID"
echo "Total compras encontradas: $CANTIDAD"
echo ""

if [ "$CANTIDAD" -gt 0 ]; then
    echo "✅ ¡ÉXITO! El cliente tiene $CANTIDAD compra(s)"
    echo ""
    echo "Detalles de las compras:"
    echo "$MIS_COMPRAS" | jq '.[] | {id, fecha, total, clienteId}'
else
    echo "❌ ERROR: No se encontraron compras"
    echo ""
    echo "🔍 Verificando directamente en la API..."
    echo "Todas las ventas (Admin):"
    
    # Intentar obtener todas las ventas para debug
    TODAS=$(curl -s -X GET "$API_URL/Ventas" -H "Authorization: Bearer $TOKEN")
    echo "$TODAS" | jq '.'
fi

echo "=========================================="

