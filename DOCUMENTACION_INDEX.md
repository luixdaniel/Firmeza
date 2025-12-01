# 📚 Índice de Documentación - Proyecto Firmeza

Este documento sirve como índice maestro de toda la documentación del proyecto.

---

## 🎯 Documentación Principal

### 📖 README.md
**Descripción**: Punto de entrada principal del proyecto
- Descripción general del sistema
- Inicio rápido con Docker
- Stack tecnológico
- Estructura del proyecto
- Roadmap y características

**Leer**: [README.md](README.md)

---

### 🏛️ ARCHITECTURE.md
**Descripción**: Arquitectura técnica del sistema
- Principios de diseño
- Diagrama de arquitectura
- Componentes del sistema
- Flujo de datos
- Patrones de diseño
- Seguridad
- Escalabilidad

**Leer**: [ARCHITECTURE.md](ARCHITECTURE.md)

---

### 🚀 DEPLOYMENT.md
**Descripción**: Guía completa de despliegue
- Prerequisitos
- Configuración inicial
- Desarrollo local
- Deployment con Docker
- Deployment en la nube (Azure, AWS, GCP)
- Variables de entorno
- Base de datos
- Troubleshooting

**Leer**: [DEPLOYMENT.md](DEPLOYMENT.md)

---

### 🤝 CONTRIBUTING.md
**Descripción**: Guía para contribuidores
- Código de conducta
- Cómo contribuir
- Configuración del entorno
- Flujo de trabajo con Git
- Estándares de código
- Tests
- Pull requests

**Leer**: [CONTRIBUTING.md](CONTRIBUTING.md)

---

### 🐳 GUIA_DOCKER.md
**Descripción**: Guía detallada de Docker
- Comandos útiles
- Estructura de docker-compose
- Volúmenes y networking
- Troubleshooting
- Best practices

**Leer**: [GUIA_DOCKER.md](GUIA_DOCKER.md)

---

## 📦 Documentación por Proyecto

### 🔌 ApiFirmeza.Web (API REST)

**Descripción**: API REST con ASP.NET Core 8.0

**README**: [ApiFirmeza.Web/README.md](ApiFirmeza.Web/README.md)

**Contenido:**
- Endpoints principales
- Autenticación JWT
- Modelos de datos
- Configuración
- Docker
- Testing

**Tecnologías:**
- ASP.NET Core 8.0 Web API
- Entity Framework Core
- PostgreSQL
- Swagger/OpenAPI
- JWT Authentication

---

### 🔧 Firmeza.Web (Portal Admin)

**Descripción**: Portal administrativo con ASP.NET Core MVC

**README**: [Firmeza.Web/README.md](Firmeza.Web/README.md)

**Contenido:**
- Funcionalidades del portal
- Áreas y controladores
- Modelos de vista
- Autorización
- Importación masiva
- Generación de PDFs

**Tecnologías:**
- ASP.NET Core 8.0 MVC
- Razor Pages
- ASP.NET Identity
- Bootstrap 5
- jQuery/DataTables

---

### 📱 firmeza-client (Cliente Web)

**Descripción**: Aplicación web cliente con Next.js 14

**README**: [firmeza-client/README.md](firmeza-client/README.md)

**Contenido:**
- Estructura del proyecto
- Funcionalidades principales
- Rutas y páginas
- Componentes
- Autenticación
- Carrito de compras
- TypeScript types

**Tecnologías:**
- Next.js 14 (App Router)
- TypeScript
- Tailwind CSS
- Context API
- Fetch API

---

### 🧪 Firmeza.Tests (Suite de Pruebas)

**Descripción**: Tests automatizados con xUnit

**README**: [Firmeza.Tests/README.md](Firmeza.Tests/README.md)

**Contenido:**
- Tipos de tests
- Estructura de tests
- Ejecutar tests
- Cobertura de código
- Mejores prácticas
- Mocking con Moq

**Tecnologías:**
- xUnit 2.4.2
- Moq 4.18.4
- EF Core InMemory
- Coverlet

---

## 🛠️ Documentos de Solución de Problemas

### SOLUCION_DOCKER_PUBLISH.md
**Descripción**: Solución al error de archivos duplicados en Docker publish

**Leer**: [SOLUCION_DOCKER_PUBLISH.md](SOLUCION_DOCKER_PUBLISH.md)

---

### SOLUCION_LOCALHOST_5090.md
**Descripción**: Solución al problema de localhost:5090 no cargando

**Leer**: [SOLUCION_LOCALHOST_5090.md](SOLUCION_LOCALHOST_5090.md)

---

### ENLACES_IMPLEMENTADOS.md
**Descripción**: Documentación de los mensajes con enlaces implementados

**Leer**: [ENLACES_IMPLEMENTADOS.md](ENLACES_IMPLEMENTADOS.md)

---

## 🎨 Scripts y Herramientas

### Scripts de Windows (.bat)

| Script | Descripción |
|--------|-------------|
| `INICIAR_DOCKER.bat` | Inicia todos los servicios con mensajes visuales |
| `verificar-docker.bat` | Verifica el estado de todos los servicios |
| `probar-api.bat` | Prueba los endpoints de la API |
| `DETENER_TODO.bat` | Detiene todos los servicios |

### Scripts de Linux/Mac (.sh)

| Script | Descripción |
|--------|-------------|
| `iniciar-docker.sh` | Inicia todos los servicios con mensajes visuales |
| `docker-start.sh` | Inicia Docker Compose |

---

## 📋 Otros Documentos

### Documentos Markdown de Configuración

| Documento | Descripción |
|-----------|-------------|
| `DOCKER_ARQUITECTURA_CORRECTA.md` | Arquitectura Docker correcta |
| `DOCKER_COMPOSE_COMPLETADO.md` | Docker Compose completado |
| `DOCKER_COMPOSE_GUIA.md` | Guía de Docker Compose |
| `DOCKER_TESTS_GUIA.md` | Guía de tests con Docker |
| `DOCKERFILES_COMPLETOS.md` | Dockerfiles completos |

### Documentos de Correcciones

| Documento | Descripción |
|-----------|-------------|
| `SOLUCION_COMPILACION_TESTS.md` | Solución a errores de compilación de tests |
| `SOLUCION_DOCKER_RED.md` | Solución a problemas de red Docker |
| `SOLUCION_ERROR_COMPILACION.md` | Solución general de errores de compilación |

### Documentos de Diseño

| Documento | Descripción |
|-----------|-------------|
| `ACTUALIZACION_DISEÑO_INDUSTRIAL.md` | Actualización de diseño industrial |
| `DISEÑO_CORREGIDO_FINAL.md` | Diseño corregido final |
| `DISEÑO_VISTAS_COMPLETO.md` | Diseño completo de vistas |
| `REDISEÑO_CLIENTE_COMPLETO.md` | Rediseño del cliente |

### Documentos de Configuración

| Documento | Descripción |
|-----------|-------------|
| `SUPABASE_CONFIGURACION.md` | Configuración de Supabase |

---

## 🔍 Cómo Navegar la Documentación

### Para Empezar
1. Lee el [README.md](README.md) principal
2. Sigue la guía de [Inicio Rápido](README.md#-inicio-rápido-5-minutos)
3. Explora la [GUIA_DOCKER.md](GUIA_DOCKER.md)

### Para Desarrollar
1. Lee [ARCHITECTURE.md](ARCHITECTURE.md) para entender el sistema
2. Lee [CONTRIBUTING.md](CONTRIBUTING.md) para estándares
3. Revisa el README del proyecto específico

### Para Desplegar
1. Lee [DEPLOYMENT.md](DEPLOYMENT.md)
2. Configura variables de entorno
3. Sigue los pasos de deployment

### Para Resolver Problemas
1. Revisa [Troubleshooting en DEPLOYMENT.md](DEPLOYMENT.md#-troubleshooting)
2. Consulta documentos de solución específicos
3. Revisa logs con `docker logs`

---

## 📊 Mapa de Dependencias entre Documentos

```
README.md (Inicio)
├── ARCHITECTURE.md (Entender sistema)
├── DEPLOYMENT.md (Desplegar)
│   ├── GUIA_DOCKER.md (Docker detallado)
│   └── SOLUCION_*.md (Problemas específicos)
├── CONTRIBUTING.md (Contribuir)
│   ├── ApiFirmeza.Web/README.md
│   ├── Firmeza.Web/README.md
│   ├── firmeza-client/README.md
│   └── Firmeza.Tests/README.md
└── ENLACES_IMPLEMENTADOS.md (Features)
```

---

## ✅ Checklist de Lectura Recomendada

### Usuario Final
- [ ] README.md - Descripción general
- [ ] GUIA_DOCKER.md - Cómo iniciar

### Desarrollador
- [ ] README.md - Descripción general
- [ ] ARCHITECTURE.md - Arquitectura del sistema
- [ ] CONTRIBUTING.md - Guía de contribución
- [ ] README del proyecto específico (API/Admin/Client/Tests)
- [ ] DEPLOYMENT.md - Solo sección de desarrollo local

### DevOps / SysAdmin
- [ ] README.md - Descripción general
- [ ] ARCHITECTURE.md - Arquitectura del sistema
- [ ] DEPLOYMENT.md - Completo
- [ ] GUIA_DOCKER.md - Completo
- [ ] Documentos de solución (SOLUCION_*.md)

### Product Owner / Manager
- [ ] README.md - Descripción general
- [ ] ARCHITECTURE.md - Secciones de alto nivel
- [ ] Roadmap en README.md

---

## 🎓 Glosario de Términos

- **API**: Application Programming Interface
- **REST**: Representational State Transfer
- **JWT**: JSON Web Token
- **MVC**: Model-View-Controller
- **ORM**: Object-Relational Mapping
- **DTO**: Data Transfer Object
- **CRUD**: Create, Read, Update, Delete
- **SSR**: Server-Side Rendering
- **SPA**: Single Page Application
- **CI/CD**: Continuous Integration/Continuous Deployment

---

## 📞 ¿Necesitas Ayuda?

Si no encuentras lo que buscas en la documentación:

1. **Busca en los documentos**: Usa Ctrl+F o búsqueda de GitHub
2. **Revisa Issues**: Puede que alguien ya haya preguntado lo mismo
3. **Crea un Issue**: [Nueva pregunta](../../issues/new)
4. **Contacta al equipo**: dev@firmeza.com

---

## 🔄 Mantenimiento de Documentación

La documentación se actualiza con cada release. Última actualización: **2025-12-01**

### Contribuir a la Documentación

¿Encontraste un error o algo no claro?
1. Edita el documento
2. Haz commit con: `docs: descripción del cambio`
3. Envía Pull Request

Ver [CONTRIBUTING.md](CONTRIBUTING.md) para más detalles.

---

**¡Gracias por usar Firmeza! 🏗️**

