# ====================================
# Dockerfile para ApiFirmeza.Web
# ====================================
#
# API REST para operaciones de backend
# Debe ejecutarse desde la RAÍZ de la solución
#
# Uso:
#   cd /ruta/a/Firmeza  (raíz de la solución)
#   docker build -f ApiFirmeza.Web/Dockerfile -t firmeza-api .
#   docker run -p 5090:8080 firmeza-api

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
COPY ["ApiFirmeza.Web/ApiFirmeza.Web.csproj", "ApiFirmeza.Web/"]
COPY ["Firmeza.Web/Firmeza.Web.csproj", "Firmeza.Web/"]

# Restaurar dependencias (ApiFirmeza.Web referencia a Firmeza.Web)
RUN dotnet restore "ApiFirmeza.Web/ApiFirmeza.Web.csproj"

# Copiar todo el código fuente
COPY ApiFirmeza.Web/ ApiFirmeza.Web/
COPY Firmeza.Web/ Firmeza.Web/

# Cambiar al directorio del proyecto API
WORKDIR /src/ApiFirmeza.Web

# Compilar en modo Release
RUN dotnet build "ApiFirmeza.Web.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build \
    --no-restore

# ====================================
# Stage 3: Publish - Publicar app
# ====================================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "ApiFirmeza.Web.csproj" \
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

# Crear directorio para recibos
RUN mkdir -p /app/recibos && chmod 755 /app/recibos

# Healthcheck para la API
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Ejecutar la aplicación API
ENTRYPOINT ["dotnet", "ApiFirmeza.Web.dll"]

