# 📂 Estructura del Frontend - Separación de Roles

## 🎯 Concepto Principal

El sistema tiene **DOS roles completamente diferentes**:

### 👨‍💼 Administrador
- **Acceso completo** a TODAS las funciones
- Puede hacer **CRUD** de clientes, productos y ventas
- Ve **TODOS los registros** del sistema
- Gestiona usuarios y configuraciones

### 🛒 Cliente
- **Acceso limitado** solo a SUS datos
- **NO puede** ver otros clientes
- **NO puede** agregar/editar/eliminar clientes
- Solo puede:
  - Ver y comprar productos
  - Ver SU historial de compras
  - Editar SU perfil

---

## 📁 Estructura de Carpetas

```
firmeza-client/app/
│
├── page.tsx                     # Página de inicio pública
├── login/                       # Login (para ambos roles)
│   └── page.tsx
│
├── admin/                       # ← SOLO ADMINISTRADORES
│   ├── layout.tsx              # Layout con sidebar
│   ├── page.tsx                # Dashboard de admin
│   ├── clientes/               # CRUD completo de clientes
│   │   └── page.tsx
│   ├── productos/              # CRUD completo de productos
│   │   └── page.tsx
│   ├── ventas/                 # Ver TODAS las ventas
│   │   └── page.tsx
│   └── configuracion/          # Configuraciones del sistema
│       └── page.tsx
│
└── cliente/                    # ← SOLO CLIENTES NORMALES
    ├── layout.tsx              # Layout simple con header
    ├── tienda/                 # Ver productos y comprar
    │   └── page.tsx
    ├── mis-compras/            # Ver SUS compras
    │   └── page.tsx
    └── perfil/                 # Ver/editar SU perfil
        └── page.tsx
```

---

## 🔒 Control de Acceso

### Rutas Protegidas

| Ruta | Admin | Cliente |
|------|-------|---------|
| `/` | Redirige a `/admin` | Redirige a `/cliente/tienda` |
| `/login` | ✅ Acceso | ✅ Acceso |
| `/admin/*` | ✅ Acceso | ❌ Bloqueado |
| `/cliente/*` | ❌ No aplica | ✅ Acceso |

### Implementación (Futuro)

```typescript
// Middleware para proteger rutas
const checkRole = () => {
  const user = JSON.parse(localStorage.getItem('user') || '{}');
  return user.role; // 'Admin' o 'Cliente'
};

// En cada layout
useEffect(() => {
  const role = checkRole();
  if (role !== 'Admin') {
    router.push('/cliente/tienda');
  }
}, []);
```

---

## 🎨 Diferencias de UI

### Layout de Admin
- **Sidebar fijo** con navegación completa
- Acceso a Dashboard, Clientes, Productos, Ventas, Configuración
- Tablas con acciones de editar/eliminar
- Botones de "Nuevo Cliente", "Nuevo Producto", etc.
- Vista de **TODOS** los registros

### Layout de Cliente
- **Header simple** con navegación básica
- Solo: Tienda, Mis Compras, Mi Perfil
- NO hay botones de editar/eliminar
- Solo ve **SUS** compras
- Carrito de compras funcional

---

## 📄 Páginas Creadas

### ✅ Admin (Completado)

1. **`/admin/page.tsx`** - Dashboard
   - Estadísticas generales
   - Gráficos (próximo)
   - Accesos rápidos

2. **`/admin/clientes/page.tsx`** - Gestión de Clientes
   - Lista de TODOS los clientes
   - Búsqueda y filtros
   - Botones de acciones (ver, editar, eliminar)
   - Estadísticas de clientes

3. **`/admin/layout.tsx`** - Layout de Admin
   - Sidebar con navegación
   - Header con logout
   - Responsive

### ✅ Cliente (Completado)

1. **`/cliente/tienda/page.tsx`** - Tienda
   - Grid de productos con imágenes
   - Búsqueda de productos
   - Agregar al carrito
   - Control de cantidad
   - Cálculo de total

2. **`/cliente/mis-compras/page.tsx`** - Historial de Compras
   - Lista de SUS compras
   - Detalles de cada pedido
   - Descargar PDF
   - Estadísticas personales

3. **`/cliente/perfil/page.tsx`** - Perfil Personal
   - Ver/editar SUS datos
   - Email, teléfono, dirección
   - Estadísticas de compras
   - Puntos acumulados

4. **`/cliente/layout.tsx`** - Layout de Cliente
   - Header simple
   - Navegación: Tienda, Mis Compras, Perfil
   - Logout

---

## 🔄 Flujo de Uso

### Como Administrador

```
1. Login con credenciales de admin
   ↓
2. Redirige a /admin (Dashboard)
   ↓
3. Puede navegar a:
   - /admin/clientes     → Ver/Editar/Eliminar TODOS los clientes
   - /admin/productos    → CRUD de productos
   - /admin/ventas       → Ver TODAS las ventas
```

### Como Cliente

```
1. Login con credenciales de cliente
   ↓
2. Redirige a /cliente/tienda
   ↓
3. Puede navegar a:
   - /cliente/tienda       → Ver productos y comprar
   - /cliente/mis-compras  → Ver SUS compras
   - /cliente/perfil       → Ver/editar SU perfil
```

---

## 🚀 Próximos Pasos

### Fase 1: Autenticación basada en roles ✅ (Parcial)
- [x] Crear layouts separados
- [x] Crear páginas de admin
- [x] Crear páginas de cliente
- [ ] Implementar verificación de rol en layouts
- [ ] Redirigir según rol después del login

### Fase 2: Funcionalidades de Admin
- [ ] CRUD completo de clientes (formularios)
- [ ] CRUD completo de productos
- [ ] Ver todas las ventas con filtros
- [ ] Exportar a Excel/PDF
- [ ] Dashboard con gráficos (Chart.js)

### Fase 3: Funcionalidades de Cliente
- [ ] Sistema de carrito persistente
- [ ] Checkout y pago simulado
- [ ] Filtros de productos por categoría
- [ ] Búsqueda avanzada
- [ ] Descargar facturas PDF

### Fase 4: Mejoras
- [ ] Notificaciones toast
- [ ] Modo oscuro
- [ ] Paginación en tablas
- [ ] Lazy loading de imágenes
- [ ] PWA (Progressive Web App)

---

## 📝 Credenciales de Prueba

### Administrador
```
Email: admin@firmeza.com
Password: Admin123$
```

### Cliente (Crear en el futuro)
```
Email: cliente@example.com
Password: Cliente123$
```

---

## 🎯 Diferencias Clave

| Característica | Admin | Cliente |
|----------------|-------|---------|
| **Ver todos los clientes** | ✅ | ❌ |
| **Agregar/Editar clientes** | ✅ | ❌ |
| **Ver todos los productos** | ✅ | ✅ |
| **Agregar/Editar productos** | ✅ | ❌ |
| **Ver todas las ventas** | ✅ | ❌ |
| **Ver sus propias compras** | N/A | ✅ |
| **Comprar productos** | N/A | ✅ |
| **Dashboard con stats** | ✅ | ❌ |
| **Editar su perfil** | ✅ | ✅ |

---

## 🛠️ Comandos para Probar

### Iniciar Frontend
```bash
cd firmeza-client
npm run dev
```

Abre: **http://localhost:3000**

### Probar Admin
1. Ir a `/login`
2. Ingresar credenciales de admin
3. Navegar a `/admin/clientes`

### Probar Cliente
1. Ir a `/login`
2. (Futuro: crear credenciales de cliente)
3. Navegar a `/cliente/tienda`

---

## ✅ Resumen

**La estructura está lista** con:

✅ Separación clara de roles (Admin vs Cliente)
✅ Layouts independientes
✅ Páginas de admin con CRUD visual
✅ Páginas de cliente con tienda y perfil
✅ Diseño responsivo con Tailwind CSS
✅ Íconos con Lucide React
✅ Protección básica de rutas

**Próximo paso:** Implementar la lógica de verificación de roles en el login para redirigir correctamente.

---

**Fecha:** 25 de noviembre de 2025
**Estado:** ✅ Estructura completada

