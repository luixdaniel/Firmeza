#!/bin/bash

echo "🚀 Preparando para subir a GitHub..."
echo "===================================="
echo ""

cd "$(dirname "$0")"

# Verificar que estamos en un repositorio Git
if [ ! -d ".git" ]; then
    echo "❌ No es un repositorio Git. Inicializando..."
    git init
    echo "✅ Repositorio Git inicializado"
fi

# Verificar que no haya credenciales expuestas
echo "🔍 Verificando seguridad..."
if git ls-files 2>/dev/null | xargs grep -il "password.*=" 2>/dev/null | grep -v "README\|SETUP\|Example\|.md$\|.gitignore" | head -1; then
    echo "⚠️  ADVERTENCIA: Posibles credenciales encontradas en archivos rastreados"
    echo "   Revisa los archivos antes de continuar"
    read -p "¿Continuar de todos modos? (y/N): " confirm
    if [[ ! $confirm =~ ^[Yy]$ ]]; then
        echo "Abortado"
        exit 1
    fi
else
    echo "✅ No se encontraron credenciales expuestas"
fi

# Verificar secrets.json en .gitignore
if grep -q "secrets.json" .gitignore; then
    echo "✅ secrets.json está protegido en .gitignore"
else
    echo "⚠️  secrets.json NO está en .gitignore"
fi

echo ""
echo "📦 Archivos que se van a subir:"
git status --short 2>/dev/null | head -20
echo ""

# Preguntar por el mensaje de commit
echo "📝 Ingresa el mensaje del commit:"
echo "   (presiona Enter para usar el mensaje por defecto)"
read -p "> " commit_message

if [ -z "$commit_message" ]; then
    commit_message="feat: Sistema completo de gestión de ventas Firmeza

- API REST con ASP.NET Core 8.0
- Portal Admin con ASP.NET MVC  
- Cliente Web con Next.js 14
- Suite de tests con xUnit
- Dockerización completa
- Documentación completa
- Sistema de email y PDFs
- Autenticación JWT + Identity
- PostgreSQL/Supabase

Listo para desarrollo y producción."
fi

# Agregar archivos
echo ""
echo "📦 Agregando archivos a Git..."
git add .

# Hacer commit
echo "💾 Haciendo commit..."
git commit -m "$commit_message"

# Verificar si hay remote configurado
if ! git remote get-url origin &>/dev/null; then
    echo ""
    echo "🌐 No hay remote configurado."
    read -p "Ingresa la URL del repositorio GitHub (o presiona Enter para saltear): " repo_url
    
    if [ -n "$repo_url" ]; then
        git remote add origin "$repo_url"
        echo "✅ Remote 'origin' agregado"
    else
        echo "⏭️  Saltado. Configura el remote manualmente con:"
        echo "   git remote add origin https://github.com/TU_USUARIO/firmeza.git"
        exit 0
    fi
fi

# Preguntar si quiere hacer push
echo ""
read -p "🚀 ¿Hacer push a GitHub ahora? (Y/n): " do_push

if [[ ! $do_push =~ ^[Nn]$ ]]; then
    echo "📤 Subiendo a GitHub..."
    
    # Intentar push
    if git push origin main 2>&1 | tee /tmp/git_push_output.txt; then
        echo ""
        echo "════════════════════════════════════════"
        echo "  ✅ ¡ÉXITO! Subido a GitHub"
        echo "════════════════════════════════════════"
        echo ""
        echo "🎉 Tu código está en GitHub"
        echo "📱 Ahora puedes:"
        echo "   - Compartir el link del repositorio"
        echo "   - Configurar GitHub Actions"
        echo "   - Invitar colaboradores"
    else
        # Si falla, intentar con -u origin main
        echo "⚠️  Intentando con 'git push -u origin main'..."
        if git push -u origin main; then
            echo ""
            echo "✅ ¡Éxito!"
        else
            echo ""
            echo "❌ Error al hacer push"
            echo "Verifica:"
            echo "  1. Que tengas permisos en el repositorio"
            echo "  2. Que hayas configurado tus credenciales de Git"
            echo "  3. Que la URL del remote sea correcta"
            echo ""
            echo "Intenta manualmente:"
            echo "  git push -u origin main"
        fi
    fi
else
    echo "⏭️  Push cancelado"
    echo ""
    echo "Para subir manualmente más tarde:"
    echo "  git push origin main"
fi

echo ""
echo "✅ ¡Listo!"

