#!/bin/bash

echo "🔍 Diagnóstico de Error de Registro - Firmeza"
echo "=============================================="
echo ""

# Verificar si la API está corriendo
echo "📡 Verificando si la API está corriendo..."
if curl -s http://localhost:5000/health > /dev/null 2>&1; then
    echo "✅ API está corriendo en puerto 5000"
    curl -s http://localhost:5000/health | jq '.' 2>/dev/null || curl -s http://localhost:5000/health
else
    echo "❌ API NO está corriendo en puerto 5000"
    echo "   Ejecuta: cd /home/Coder/Escritorio/Firmeza/ApiFirmeza.Web && dotnet run"
fi

echo ""
echo "🔐 Intentando registrar un usuario de prueba..."
echo ""

# Intentar registro
response=$(curl -s -w "\nHTTP_STATUS:%{http_code}" -X POST http://localhost:5000/api/Auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test-'$(date +%s)'@test.com",
    "password": "Test123$",
    "confirmPassword": "Test123$",
    "nombre": "Test",
    "apellido": "Usuario",
    "telefono": "+57 300 123 4567"
  }')

http_status=$(echo "$response" | grep "HTTP_STATUS" | cut -d':' -f2)
body=$(echo "$response" | sed '/HTTP_STATUS/d')

echo "Status HTTP: $http_status"
echo "Respuesta:"
echo "$body" | jq '.' 2>/dev/null || echo "$body"

echo ""
echo "📋 Posibles causas de error:"
echo "  1. ❌ El rol 'Cliente' no existe en la base de datos"
echo "  2. ❌ La base de datos no está corriendo"
echo "  3. ❌ Las migraciones no se han aplicado"
echo "  4. ❌ La cadena de conexión es incorrecta"
echo ""
echo "🔧 Soluciones sugeridas:"
echo "  1. Reiniciar la API (presiona Ctrl+C y ejecuta 'dotnet run' de nuevo)"
echo "  2. Aplicar migraciones: cd ApiFirmeza.Web && dotnet ef database update"
echo "  3. Verificar que PostgreSQL esté corriendo"
echo ""

