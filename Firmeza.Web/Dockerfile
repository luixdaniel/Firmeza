# ====================================
# Dockerfile para Firmeza.Web
# ====================================
#
# Portal MVC principal para gestión administrativa
# Debe ejecutarse desde la RAÍZ de la solución
#
# Uso:
#   cd /ruta/a/Firmeza  (raíz de la solución)
#   docker build -f Firmeza.Web/Dockerfile -t firmeza-web .
#   docker run -p 5000:8080 firmeza-web

# ====================================
# Stage 1: Base - Runtime ASP.NET
# ====================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Variables de entorno
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# ====================================
# Stage 2: Build - SDK para compilar
# ====================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos de proyecto (.csproj) para cachear restore
COPY ["Firmeza.Web/Firmeza.Web.csproj", "Firmeza.Web/"]

# Restaurar dependencias
RUN dotnet restore "Firmeza.Web/Firmeza.Web.csproj"

# Copiar todo el código fuente
COPY Firmeza.Web/ Firmeza.Web/

# Cambiar al directorio del proyecto
WORKDIR /src/Firmeza.Web

# Compilar en modo Release
RUN dotnet build "Firmeza.Web.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build \
    --no-restore

# ====================================
# Stage 3: Publish - Publicar app
# ====================================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Firmeza.Web.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ====================================
# Stage 4: Final - Imagen runtime
# ====================================
FROM base AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copiar archivos publicados
COPY --from=publish /app/publish .

# Healthcheck
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "Firmeza.Web.dll"]
