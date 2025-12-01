#!/bin/bash

# ====================================
#   INICIAR APLICACIÓN FIRMEZA
# ====================================

set -e  # Salir si hay error

echo ""
echo "========================================"
echo "   FIRMEZA - INICIAR APLICACIÓN COMPLETA"
echo "========================================"
echo ""

cd "$(dirname "$0")"

echo "[INFO] Directorio: $(pwd)"
echo ""
echo "FLUJO DE EJECUCIÓN:"
echo "  1. Tests          - Pruebas automatizadas (OBLIGATORIO)"
echo "  2. Base de Datos  - PostgreSQL (Puerto 5432)"
echo "  3. API            - REST API (Puerto 5090)"
echo "  4. Admin          - Portal Web (Puerto 5000)"
echo "  5. Cliente        - Frontend SPA (Puerto 3000)"
echo ""
echo "IMPORTANTE:"
echo "  - Si los tests FALLAN, todo se detiene"
echo "  - Si los tests PASAN, se levantan todos los servicios"
echo ""
echo "Presiona Ctrl+C para detener todos los servicios"
echo ""

# Ejecutar docker-compose
docker-compose up --build

# Verificar código de salida
EXIT_CODE=$?

if [ $EXIT_CODE -ne 0 ]; then
    echo ""
    echo "[ERROR] Fallo en el despliegue"
    echo "[ERROR] Código de salida: $EXIT_CODE"
    echo ""
    echo "Posibles causas:"
    echo "  - Tests fallaron"
    echo "  - Error de compilación"
    echo "  - Docker no está corriendo"
    echo ""
    exit $EXIT_CODE
fi

echo ""
echo "[ÉXITO] Aplicación iniciada correctamente"
echo ""

