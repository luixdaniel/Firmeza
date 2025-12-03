# ====================================
# Dockerfile para Firmeza.Tests
# ====================================
#
# Ejecuta la suite de pruebas automatizadas
# Debe ejecutarse desde la RAÍZ de la solución
#
# Uso:
#   cd /ruta/a/Firmeza  (raíz de la solución)
#   docker build -f Firmeza.Tests/Dockerfile -t firmeza-tests .
#   docker run firmeza-tests
#
# Este contenedor:
#   1. Restaura dependencias
#   2. Compila el proyecto de tests
#   3. Ejecuta todos los tests
#   4. Sale con código 0 (éxito) o 1 (fallo)

# ====================================
# Stage 1: Build and Test
# ====================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de solución y proyectos para restore
COPY ["Firmeza.Web/Firmeza.Web.csproj", "Firmeza.Web/"]
COPY ["Firmeza.Tests/Firmeza.Tests.csproj", "Firmeza.Tests/"]

# Restaurar dependencias de ambos proyectos
RUN dotnet restore "Firmeza.Web/Firmeza.Web.csproj"
RUN dotnet restore "Firmeza.Tests/Firmeza.Tests.csproj"

# Copiar el código fuente completo
COPY Firmeza.Web/ Firmeza.Web/
COPY Firmeza.Tests/ Firmeza.Tests/

# Cambiar al directorio de tests
WORKDIR /src/Firmeza.Tests

# Compilar el proyecto de tests
RUN dotnet build "Firmeza.Tests.csproj" \
    -c Release \
    --no-restore

# ====================================
# Stage 2: Test Execution
# ====================================
FROM build AS test

# Crear directorio para resultados
RUN mkdir -p /app/test-results

# Variables de entorno para .NET CLI
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# Ejecutar tests con logger detallado
# Si los tests fallan, el contenedor sale con código de error != 0
ENTRYPOINT ["dotnet", "test", "Firmeza.Tests.csproj", \
    "--no-build", \
    "--configuration", "Release", \
    "--logger", "trx;LogFileName=/app/test-results/test-results.trx", \
    "--logger", "console;verbosity=detailed", \
    "--results-directory", "/app/test-results"]

