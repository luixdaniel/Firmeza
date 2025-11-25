# 🎨 Firmeza Client - Frontend Next.js

Frontend moderno construido con Next.js 14, TypeScript y Tailwind CSS para el sistema de gestión Firmeza.

## 🚀 Stack Tecnológico

- **Framework:** Next.js 14 (App Router)
- **Lenguaje:** TypeScript
- **Styling:** Tailwind CSS
- **HTTP Client:** Axios
- **State Management:** React Hooks
- **Autenticación:** JWT

## 📋 Requisitos Previos

- Node.js 18+ instalado
- npm o pnpm
- La API de Firmeza corriendo en `http://localhost:5090`

## ⚙️ Instalación

1. **Instalar dependencias:**
   ```bash
   cd firmeza-client
   npm install
   ```

2. **Configurar variables de entorno:**
   
   Crea un archivo `.env.local` en la raíz del proyecto:
   ```env
   NEXT_PUBLIC_API_URL=http://localhost:5090
   ```

## 🎯 Ejecutar el Proyecto

### Modo Desarrollo
```bash
npm run dev
```

La aplicación estará disponible en: **http://localhost:3000**

### Modo Producción
```bash
npm run build
npm start
```

## 📁 Estructura del Proyecto

```
firmeza-client/
├── app/                    # App Router de Next.js
│   ├── layout.tsx         # Layout principal
│   ├── page.tsx           # Página de inicio
│   ├── login/             # Página de login
│   ├── clientes/          # Gestión de clientes
│   ├── productos/         # Gestión de productos
│   ├── ventas/            # Gestión de ventas
│   └── dashboard/         # Dashboard principal
├── components/            # Componentes reutilizables
├── services/              # Servicios API
│   └── api.ts            # Llamadas a la API REST
├── types/                # TypeScript types/interfaces
│   └── index.ts          # Definiciones de tipos
├── lib/                  # Utilidades
│   └── axios.ts          # Configuración de Axios
└── public/               # Archivos estáticos

```

## 🔐 Autenticación

El sistema usa JWT (JSON Web Tokens) para autenticación:

1. **Login:** El usuario ingresa credenciales en `/login`
2. **Token:** La API devuelve un JWT que se guarda en `localStorage`
3. **Protección:** Cada petición incluye el token en el header `Authorization`
4. **Expiración:** Si el token expira, el usuario es redirigido a `/login`

### Credenciales de Prueba

```
Email: admin@firmeza.com
Password: Admin123$
```

## 📄 Páginas Implementadas

### ✅ Página de Inicio (`/`)
- Landing page con enlaces a todas las secciones
- Descripción del sistema

### ✅ Login (`/login`)
- Formulario de autenticación
- Manejo de errores
- Redirección automática al dashboard

### ✅ Clientes (`/clientes`)
- Lista completa de clientes
- Estadísticas (total, activos, inactivos)
- Tabla con información detallada
- **Próximamente:** Crear, editar, eliminar

### 🚧 Productos (`/productos`)
- Vista de productos
- Gestión de inventario

### 🚧 Ventas (`/ventas`)
- Registro de ventas
- Historial de transacciones

### 🚧 Dashboard (`/dashboard`)
- Resumen general
- Gráficos y estadísticas

## 🔧 Servicios API

Todos los servicios están en `services/api.ts`:

```typescript
// Autenticación
authService.login(credentials)
authService.getMe()

// Clientes
clientesService.getAll()
clientesService.getById(id)
clientesService.create(cliente)
clientesService.update(id, cliente)
clientesService.delete(id)

// Ventas
ventasService.getAll()
ventasService.getById(id)
ventasService.create(venta)

// Productos
productosService.getAll()
productosService.getById(id)
productosService.create(producto)
productosService.update(id, producto)
productosService.delete(id)
```

## 🎨 Personalización

### Colores

Los colores están definidos en `tailwind.config.js`:

```javascript
colors: {
  primary: {
    50: '#f0f9ff',
    // ...
    600: '#0284c7', // Color principal
    // ...
  }
}
```

### Estilos Globales

Los estilos globales están en `app/globals.css`.

## 🐛 Solución de Problemas

### Error: Cannot find module 'autoprefixer'

Este error ocurre cuando faltan las dependencias de PostCSS/Autoprefixer.

**Solución:**
```bash
npm install autoprefixer postcss
npm run dev
```

**Verificar instalación:**
```bash
npm list autoprefixer postcss
```

### Error: Cannot connect to API

**Solución:** Asegúrate de que la API esté corriendo en `http://localhost:5090`

```bash
cd ApiFirmeza.Web
dotnet run
```

### Error: 401 Unauthorized

**Causas posibles:**
1. Token expirado → Vuelve a hacer login
2. Token inválido → Limpia localStorage y vuelve a hacer login
3. API no está configurada correctamente

### Error: Module not found

**Solución:**
```bash
rm -rf node_modules package-lock.json
npm install
```

## 📦 Scripts Disponibles

```bash
npm run dev      # Ejecutar en modo desarrollo
npm run build    # Compilar para producción
npm run start    # Ejecutar versión de producción
npm run lint     # Ejecutar linter
```

## 🚀 Próximas Funcionalidades

- [ ] Página de Dashboard con gráficos
- [ ] CRUD completo de Clientes
- [ ] CRUD completo de Productos
- [ ] Crear ventas con carrito
- [ ] Búsqueda y filtros
- [ ] Exportar a PDF/Excel
- [ ] Paginación
- [ ] Modo oscuro
- [ ] Notificaciones toast
- [ ] Validación de formularios con Zod

## 📚 Recursos

- [Next.js Documentation](https://nextjs.org/docs)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Axios Documentation](https://axios-http.com/docs/intro)

## 🎯 Checklist de Inicio

- [ ] API corriendo en http://localhost:5090
- [ ] Dependencias instaladas (`npm install`)
- [ ] Archivo `.env.local` configurado
- [ ] Proyecto corriendo (`npm run dev`)
- [ ] Login funcional
- [ ] Conexión con API verificada

---

**¡Listo para desarrollar!** 🎉

Si tienes problemas, revisa:
1. Que la API esté corriendo
2. Que las credenciales sean correctas
3. Los logs en la consola del navegador (F12)

