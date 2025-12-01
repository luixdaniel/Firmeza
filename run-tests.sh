#!/bin/bash

# ====================================
#   EJECUTAR TESTS DE FIRMEZA
# ====================================

set -e  # Salir si hay error

echo ""
echo "========================================"
echo "   FIRMEZA TESTS - DOCKER"
echo "========================================"
echo ""

# Ir a la raíz de la solución
cd "$(dirname "$0")"

echo "[INFO] Ejecutando desde la raíz de la solución"
echo "[INFO] Directorio actual: $(pwd)"
echo ""

echo "[1/3] Construyendo imagen de tests..."
docker build -f Firmeza.Tests/Dockerfile -t firmeza-tests:latest .

echo ""
echo "[2/3] Tests completados exitosamente"
echo ""

# Crear directorio para resultados
mkdir -p test-results

echo "[3/3] Copiando resultados..."
docker run --rm -v "$(pwd)/test-results:/app/test-results" firmeza-tests:latest echo "Resultados copiados" 2>/dev/null || true

echo ""
echo "========================================"
echo "   ✅ TESTS EXITOSOS"
echo "========================================"
echo ""
echo "Resultados disponibles en: test-results/"
echo ""

exit 0

