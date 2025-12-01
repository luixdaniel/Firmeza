# ====================================
# Dockerfile para firmeza-client
# ====================================
#
# Frontend Next.js con React
# Este Dockerfile puede ejecutarse desde su propio directorio
#
# Uso:
#   cd firmeza-client
#   docker build -t firmeza-client .
#   docker run -p 3000:3000 firmeza-client

# ====================================
# Stage 1: Dependencies - Instalar deps
# ====================================
FROM node:18-alpine AS deps

# Instalar libc6-compat para compatibilidad
RUN apk add --no-cache libc6-compat

WORKDIR /app

# Copiar archivos de dependencias
COPY package.json package-lock.json* ./

# Instalar dependencias (producción y desarrollo)
RUN npm ci

# ====================================
# Stage 2: Builder - Compilar Next.js
# ====================================
FROM node:18-alpine AS builder

WORKDIR /app

# Copiar node_modules del stage anterior
COPY --from=deps /app/node_modules ./node_modules

# Copiar código fuente
COPY . .

# Variables de entorno para build
ENV NEXT_TELEMETRY_DISABLED 1
ENV NODE_ENV production

# Build de Next.js
RUN npm run build

# ====================================
# Stage 3: Runner - Ejecutar app
# ====================================
FROM node:18-alpine AS runner

WORKDIR /app

ENV NODE_ENV production
ENV NEXT_TELEMETRY_DISABLED 1

# Crear usuario no-root
RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

# Copiar archivos de build standalone
COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

# Copiar public solo si existe (Next.js App Router puede no tenerlo)
# Si no existe, Docker lo ignorará sin fallar
RUN mkdir -p ./public

# Cambiar a usuario no-root
USER nextjs

# Exponer puerto
EXPOSE 3000

ENV PORT 3000
ENV HOSTNAME "0.0.0.0"

# Healthcheck
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD node -e "require('http').get('http://localhost:3000/api/health', (r) => {process.exit(r.statusCode === 200 ? 0 : 1)})" || exit 1

# Ejecutar Next.js
CMD ["node", "server.js"]

