# 🏗️ Firmeza - Sistema de Gestión de Ventas

> Sistema integral de gestión de ventas de insumos de construcción, desarrollado con arquitectura de microservicios.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Next.js](https://img.shields.io/badge/Next.js-14-black?logo=next.js)](https://nextjs.org/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

---

## 📋 Descripción

Firmeza es un sistema moderno de gestión de ventas que permite:
- 🛒 **Clientes**: Navegar catálogo, comprar productos, ver historial
- 👨‍💼 **Administradores**: Gestionar productos, categorías, clientes y ventas
- 📊 **Reportes**: Generar recibos PDF, estadísticas de ventas
- 📧 **Notificaciones**: Emails automáticos de confirmación

---

## 🚀 Inicio Rápido (5 minutos)

### Prerequisitos
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y corriendo

### Iniciar el Proyecto

**Windows:**
```bash
INICIAR_DOCKER.bat
```

**Linux/Mac:**
```bash
chmod +x iniciar-docker.sh
./iniciar-docker.sh
```

**O manualmente:**
```bash
docker-compose up --build -d
```

### Acceder a los Servicios

| Servicio | URL | Credenciales |
|----------|-----|-------------|
| 📱 **Cliente** | http://localhost:3000 | - |
| 🔧 **Admin** | http://localhost:5000 | `admin@firmeza.com` / `Admin123$` |
| 🔌 **API** | http://localhost:5090/swagger | (usar token JWT) |

---

## 📚 Documentación Completa

### 📖 Documentos Principales

| Documento | Descripción |
|-----------|-------------|
| [**ARCHITECTURE.md**](ARCHITECTURE.md) | 🏛️ Arquitectura del sistema, patrones y flujos |
| [**DEPLOYMENT.md**](DEPLOYMENT.md) | 🚀 Guía completa de despliegue (local, cloud, VPS) |
| [**CONTRIBUTING.md**](CONTRIBUTING.md) | 🤝 Cómo contribuir al proyecto |
| [**GUIA_DOCKER.md**](GUIA_DOCKER.md) | 🐳 Guía detallada de Docker |

### 📦 Documentación por Proyecto

| Proyecto | README | Descripción |
|----------|--------|-------------|
| **ApiFirmeza.Web** | [README](ApiFirmeza.Web/README.md) | 🔌 API REST con ASP.NET Core |
| **Firmeza.Web** | [README](Firmeza.Web/README.md) | 🔧 Portal Admin con ASP.NET MVC |
| **firmeza-client** | [README](firmeza-client/README.md) | 📱 Cliente Web con Next.js |
| **Firmeza.Tests** | [README](Firmeza.Tests/README.md) | 🧪 Suite de pruebas con xUnit |

---

## 🏗️ Arquitectura

```
┌─────────────────────────────────────────────────────────────────┐
│                         USUARIOS                                │
├──────────────┬────────────────────┬────────────────────────────┤
│   Clientes   │   Administradores  │      Desarrolladores       │
└──────┬───────┴─────────┬──────────┴────────────┬───────────────┘
       │                 │                       │
       ▼                 ▼                       ▼
┌──────────────┐  ┌──────────────┐      ┌──────────────┐
│   Cliente    │  │    Admin     │      │   Swagger    │
│   Next.js    │  │   ASP.NET    │      │   /swagger   │
│  Port: 3000  │  │  Port: 5000  │      │  Port: 5090  │
└──────┬───────┘  └──────┬───────┘      └──────┬───────┘
       │                 │                     │
       └─────────────────┼─────────────────────┘
                         │
                         ▼
                 ┌───────────────┐
                 │   API REST    │
                 │  ASP.NET Core │
                 │  Port: 5090   │
                 └───────┬───────┘
                         │
                         ▼
                 ┌───────────────┐
                 │  PostgreSQL   │
                 │  Port: 5432   │
                 └───────────────┘
```

Ver detalles completos en [ARCHITECTURE.md](ARCHITECTURE.md)

---

## 🛠️ Stack Tecnológico

### Backend
- **API REST**: ASP.NET Core 8.0 Web API
- **Portal Admin**: ASP.NET Core 8.0 MVC
- **ORM**: Entity Framework Core 8.0
- **Base de Datos**: PostgreSQL 16 (Supabase)
- **Autenticación**: JWT + ASP.NET Identity
- **PDF**: iTextSharp
- **Email**: MailKit + MimeKit

### Frontend
- **Framework**: Next.js 14 (App Router)
- **Lenguaje**: TypeScript
- **Estilos**: Tailwind CSS
- **UI**: Headless UI, Heroicons
- **Estado**: Context API

### DevOps
- **Contenedores**: Docker + Docker Compose
- **CI/CD**: GitHub Actions (próximamente)
- **Tests**: xUnit + Moq
- **Documentation**: Swagger/OpenAPI

---

## 📁 Estructura del Proyecto

```
Firmeza/
├── 📄 README.md                    # Este archivo
├── 📄 ARCHITECTURE.md              # Documentación de arquitectura
├── 📄 DEPLOYMENT.md                # Guía de despliegue
├── 📄 CONTRIBUTING.md              # Guía de contribución
├── 🐳 docker-compose.yml            # Orquestación Docker
├── 🐳 .env.example                  # Template de variables
│
├── 🔌 ApiFirmeza.Web/              # API REST
│   ├── Controllers/                # Endpoints HTTP
│   ├── Services/                   # Lógica de negocio
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Dockerfile                  # Imagen Docker
│   └── README.md                   # Documentación de la API
│
├── 🔧 Firmeza.Web/                 # Portal Administrativo
│   ├── Areas/Admin/                # Área de administración
│   ├── Data/                       # Entities y DbContext
│   ├── Repositories/               # Acceso a datos
│   ├── Services/                   # Servicios de negocio
│   ├── Dockerfile                  # Imagen Docker
│   └── README.md                   # Documentación del Admin
│
├── 📱 firmeza-client/              # Cliente Web
│   ├── app/                        # Next.js App Router
│   ├── components/                 # Componentes React
│   ├── services/                   # Servicios API
│   ├── contexts/                   # Context API
│   ├── Dockerfile                  # Imagen Docker
│   └── README.md                   # Documentación del Cliente
│
└── 🧪 Firmeza.Tests/               # Suite de Pruebas
    ├── Controllers/                # Tests de controladores
    ├── Services/                   # Tests de servicios
    ├── Repositories/               # Tests de repositorios
    ├── Dockerfile                  # Imagen Docker de tests
    └── README.md                   # Documentación de tests
```

---

## ⚡ Funcionalidades

### Para Clientes 🛒
- ✅ Navegación de catálogo de productos
- ✅ Filtrado por categorías
- ✅ Búsqueda de productos
- ✅ Carrito de compras
- ✅ Proceso de checkout
- ✅ Historial de compras
- ✅ Descarga de recibos PDF
- ✅ Gestión de perfil

### Para Administradores 👨‍💼
- ✅ Dashboard con estadísticas
- ✅ CRUD de productos
- ✅ CRUD de categorías
- ✅ Gestión de clientes
- ✅ Gestión de ventas
- ✅ Generación de reportes
- ✅ Importación masiva Excel
- ✅ Generación de recibos PDF

### Características Técnicas 🔧
- ✅ Autenticación JWT
- ✅ Autorización basada en roles
- ✅ Validaciones client-side y server-side
- ✅ Emails automáticos
- ✅ Health checks
- ✅ API documentada con Swagger
- ✅ Tests automatizados
- ✅ Containerización con Docker

---

## 🔐 Seguridad

- 🔒 Autenticación JWT con tokens seguros
- 🔒 Passwords hasheados con ASP.NET Identity
- 🔒 Validación de roles y permisos
- 🔒 CORS configurado
- 🔒 Protección contra SQL Injection (EF Core)
- 🔒 HTTPS recomendado en producción
- 🔒 Variables sensibles en archivos .env

---

## 📦 Instalación y Configuración

### 1. Clonar Repositorio

```bash
git clone https://github.com/tu-usuario/firmeza.git
cd firmeza
```

### 2. Configurar Variables de Entorno

```bash
# Copiar template
cp .env.example .env

# Editar con tus credenciales
nano .env
```

Ver guía completa en [DEPLOYMENT.md](DEPLOYMENT.md)

### 3. Iniciar con Docker

```bash
# Build y start
docker-compose up --build -d

# Ver logs
docker-compose logs -f

# Verificar estado
docker-compose ps
```

### 4. Verificar Servicios

```bash
# API
curl http://localhost:5090/health

# Admin
curl http://localhost:5000/health

# Cliente
curl http://localhost:3000
```

---

## 🧪 Testing

### Ejecutar Tests

```bash
# Localmente
dotnet test

# Con Docker
docker-compose run tests

# Con cobertura
dotnet test /p:CollectCoverage=true
```

Ver más en [Firmeza.Tests/README.md](Firmeza.Tests/README.md)

---

## 🚀 Deployment

### Desarrollo Local
```bash
docker-compose up --build -d
```

### Producción

**Opciones soportadas:**
- Azure Container Instances
- AWS ECS/Fargate
- Google Cloud Run
- DigitalOcean/Linode VPS
- Kubernetes

Ver guía completa en [DEPLOYMENT.md](DEPLOYMENT.md)

---

## 📊 Roadmap

### v1.0 (Actual) ✅
- [x] API REST completa
- [x] Portal administrativo
- [x] Cliente web
- [x] Autenticación y autorización
- [x] Generación de PDFs
- [x] Emails automáticos
- [x] Containerización Docker
- [x] Suite de tests

### v1.1 (Próximo) 🔄
- [ ] Dashboard con gráficos avanzados
- [ ] Reportes de ventas en Excel
- [ ] Imágenes de productos
- [ ] Recuperación de contraseña
- [ ] Notificaciones en tiempo real

### v2.0 (Futuro) 🔮
- [ ] Aplicación móvil
- [ ] Sistema de puntos/fidelización
- [ ] Integración con pasarelas de pago
- [ ] Multi-tenant
- [ ] API v2 con GraphQL

---

## 🤝 Contribuir

¡Las contribuciones son bienvenidas! Por favor lee [CONTRIBUTING.md](CONTRIBUTING.md) para detalles sobre nuestro código de conducta y el proceso para enviar pull requests.

### Pasos Rápidos

1. Fork el proyecto
2. Crea tu rama (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'feat: Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver [LICENSE](LICENSE) para más detalles.

---

## 👥 Equipo

- **Desarrollador Principal**: [Tu Nombre](https://github.com/luixdaniel)
- **Contribuidores**: Ver [CONTRIBUTORS.md](CONTRIBUTORS.md)

---

## 📞 Contacto y Soporte

- 📧 **Email**: ceraluis4@gmail.com
- 💬 **Issues**: [GitHub Issues](https://github.com/luixdaniel/firmeza/issues)
- 📖 **Wiki**: [GitHub Wiki](https://github.com/luixdaniel/firmeza/wiki)

---

## 🙏 Agradecimientos

- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [Next.js](https://nextjs.org/)
- [Supabase](https://supabase.com/)
- [Tailwind CSS](https://tailwindcss.com/)
- [Docker](https://www.docker.com/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

---

## 📈 Estado del Proyecto

- ✅ **Estable**: Listo para uso en producción
- 🔄 **Activo**: En desarrollo activo
- 📝 **Documentado**: Documentación completa disponible
- 🧪 **Testeado**: Suite de tests implementada
- 🐳 **Docker**: Totalmente containerizado

---

## 🎯 Características Destacadas

### 🔥 Hot Features

- **API First**: Todas las funcionalidades expuestas vía API REST
- **Responsive**: Funciona en desktop, tablet y móvil
- **Real-time**: Actualizaciones en tiempo real (próximamente)
- **Scalable**: Arquitectura preparada para escalar horizontalmente
- **Tested**: Cobertura de tests >80%
- **Documented**: Swagger/OpenAPI + README completos
- **Docker Ready**: Un comando para deployar todo
- **Cloud Native**: Listo para AWS, Azure, GCP

---

## 📸 Screenshots

> Próximamente: Screenshots de la aplicación

---

## 🌟 ¡Dale una estrella!

Si este proyecto te ha sido útil, considera darle una ⭐ en GitHub. ¡Gracias!

---

**Hecho con ❤️ usando .NET, Next.js y Docker**

*Última actualización: Diciembre 2025*

