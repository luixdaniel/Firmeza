#!/bin/bash

# Script para iniciar los portales de Firmeza correctamente separados
# Este script inicia los tres componentes en terminales separadas

echo "╔════════════════════════════════════════════════════════════╗"
echo "║       SISTEMA FIRMEZA - INICIAR PORTALES SEPARADOS        ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Colores
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Verificar si estamos en el directorio correcto
if [ ! -d "ApiFirmeza.Web" ] || [ ! -d "Firmeza.Web" ] || [ ! -d "firmeza-client" ]; then
    echo "❌ Error: Este script debe ejecutarse desde el directorio raíz del proyecto Firmeza"
    exit 1
fi

echo "📋 PORTALES QUE SE INICIARÁN:"
echo ""
echo -e "${BLUE}1. API REST (ApiFirmeza.Web)${NC}"
echo "   - Puerto: http://localhost:5000"
echo "   - Propósito: Backend para portal de clientes"
echo "   - Autenticación: JWT"
echo ""
echo -e "${GREEN}2. Portal de Administradores (Firmeza.Web)${NC}"
echo "   - Puerto: http://localhost:5002"
echo "   - Propósito: Gestión administrativa con Razor"
echo "   - Autenticación: ASP.NET Core Identity"
echo "   - Usuarios: SOLO ADMINISTRADORES"
echo ""
echo -e "${YELLOW}3. Portal de Clientes (firmeza-client)${NC}"
echo "   - Puerto: http://localhost:3000"
echo "   - Propósito: Portal web para clientes"
echo "   - Autenticación: JWT (consume la API)"
echo "   - Usuarios: SOLO CLIENTES"
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""

# Función para iniciar en terminal nueva (Linux)
start_in_new_terminal() {
    local title=$1
    local command=$2
    
    # Intentar con diferentes emuladores de terminal
    if command -v gnome-terminal &> /dev/null; then
        gnome-terminal --title="$title" -- bash -c "$command; exec bash"
    elif command -v konsole &> /dev/null; then
        konsole --title "$title" -e bash -c "$command; exec bash" &
    elif command -v xterm &> /dev/null; then
        xterm -title "$title" -e bash -c "$command; exec bash" &
    else
        echo "⚠️  No se encontró un emulador de terminal. Ejecuta manualmente:"
        echo "   $command"
    fi
}

# Preguntar al usuario
read -p "¿Iniciar todos los portales ahora? (s/n): " -n 1 -r
echo ""
if [[ ! $REPLY =~ ^[Ss]$ ]]; then
    echo "❌ Cancelado. Puedes iniciar manualmente cada portal."
    exit 0
fi

echo ""
echo "🚀 Iniciando portales..."
echo ""

# 1. Iniciar API
echo -e "${BLUE}[1/3] Iniciando API REST...${NC}"
start_in_new_terminal "API REST - ApiFirmeza.Web" "cd ApiFirmeza.Web && dotnet run"
sleep 2

# 2. Iniciar Portal Admin
echo -e "${GREEN}[2/3] Iniciando Portal de Administradores...${NC}"
start_in_new_terminal "Portal Admin - Firmeza.Web" "cd Firmeza.Web && dotnet run"
sleep 2

# 3. Iniciar Portal Cliente
echo -e "${YELLOW}[3/3] Iniciando Portal de Clientes...${NC}"
cd firmeza-client
if [ ! -d "node_modules" ]; then
    echo "📦 Instalando dependencias de Node.js..."
    npm install
fi
start_in_new_terminal "Portal Cliente - firmeza-client" "cd firmeza-client && npm run dev"

echo ""
echo "✅ PORTALES INICIADOS"
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "📌 URLS DE ACCESO:"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""
echo -e "${BLUE}🔌 API REST:${NC}"
echo "   http://localhost:5000"
echo "   Swagger: http://localhost:5000/swagger"
echo ""
echo -e "${GREEN}👨‍💼 PORTAL ADMINISTRADORES:${NC}"
echo "   http://localhost:5002"
echo "   Login: http://localhost:5002/Identity/Account/Login"
echo "   Credenciales: admin@firmeza.com / Admin123$"
echo ""
echo -e "${YELLOW}👥 PORTAL CLIENTES:${NC}"
echo "   http://localhost:3000"
echo "   Login: http://localhost:3000/login"
echo "   Credenciales: cliente@firmeza.com / Cliente123$"
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""
echo "⚠️  IMPORTANTE:"
echo "   • Los portales están SEPARADOS y NO deben mezclarse"
echo "   • Admins SOLO usan el puerto 5002 (Razor)"
echo "   • Clientes SOLO usan el puerto 3000 (Next.js)"
echo "   • La API (5000) es consumida solo por el portal de clientes"
echo ""
echo "📚 Más información: Lee ARQUITECTURA_PORTALES.md"
echo ""
echo "Para detener todos los servicios, cierra las ventanas de terminal."

