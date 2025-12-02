#!/bin/bash
echo "   git push origin main"
echo "   git commit -m 'tu mensaje'"
echo "   git add ."
echo "📋 Siguiente paso:"
echo ""
echo "✅ Limpieza completada!"
echo ""

fi
    echo "✅ No se encontraron credenciales expuestas"
else
    echo "   Revisa estos archivos antes de hacer commit"
    echo "⚠️  ADVERTENCIA: Se encontraron archivos con posibles credenciales"
if git ls-files | xargs grep -l "Password=" 2>/dev/null | grep -v ".gitignore\|README\|SETUP\|.md$"; then
# Buscar posibles contraseñas o conexiones

echo "🔍 Verificando que no haya credenciales en archivos rastreados..."
echo ""
# Verificar que no haya credenciales en archivos rastreados

fi
    rm -rf firmeza-client/.next
    rm -rf firmeza-client/node_modules
    echo "🗑️  Limpiando node_modules (será reinstalado en build)..."
if [ -d "firmeza-client/node_modules" ]; then
# Limpiar node_modules si existe

done
    fi
        rm -rf "$dir"
    if [[ "$dir" != *"node_modules"* ]]; then
find . -type d -name "bin" -o -name "obj" | while read dir; do
echo "🗑️  Limpiando directorios de compilación..."
# Limpiar bins y objs

find . -name "secrets.json" -not -path "*/usersecrets/*" -delete
find . -name "*.secret.*" -delete
echo "🗑️  Eliminando archivos de configuración temporal..."
# Eliminar archivos de configuración temporal

rm -f SOLUCION_GMAIL_SMTP_LINUX.md
rm -f DIAGNOSTICO_FINAL_GMAIL.md
rm -f verificar_gmail.sh
rm -f test_smtp_connection.sh
rm -rf TestGmailSMTP/
echo "🗑️  Eliminando archivos de prueba temporal..."
# Eliminar archivos de prueba temporal

cd "$(dirname "$0")"

echo ""
echo "================================================"
echo "🧹 Limpiando archivos temporales antes de Git..."


