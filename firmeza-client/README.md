# 📱 firmeza-client - Cliente Web (Next.js)

## 📋 Descripción

Aplicación web cliente desarrollada en **Next.js 14** con **TypeScript** y **Tailwind CSS**. Proporciona una interfaz moderna y responsive para que los clientes naveguen el catálogo de productos, gestionen su carrito de compras y realicen pedidos.

---

## 🏗️ Tecnologías

- **Framework:** Next.js 14 (App Router)
- **Lenguaje:** TypeScript
- **Estilos:** Tailwind CSS
- **UI Components:** Headless UI, Heroicons
- **Estado:** Context API + localStorage
- **HTTP Client:** Fetch API nativo
- **Autenticación:** JWT (Bearer Token)
- **Deployment:** Docker

---

## 📁 Estructura del Proyecto

```
firmeza-client/
├── app/                       # App Router de Next.js 14
│   ├── layout.tsx             # Layout principal
│   ├── page.tsx               # Página de inicio
│   ├── productos/             # Catálogo de productos
│   │   ├── page.tsx           # Lista de productos
│   │   └── [id]/              # Detalle de producto
│   ├── carrito/               # Carrito de compras
│   │   └── page.tsx
│   ├── checkout/              # Proceso de compra
│   │   └── page.tsx
│   ├── mis-compras/           # Historial de compras
│   │   └── page.tsx
│   ├── login/                 # Iniciar sesión
│   │   └── page.tsx
│   ├── registro/              # Registro de usuario
│   │   └── page.tsx
│   └── perfil/                # Perfil del usuario
│       └── page.tsx
├── components/                # Componentes reutilizables
│   ├── Navbar.tsx             # Barra de navegación
│   ├── Footer.tsx             # Pie de página
│   ├── ProductCard.tsx        # Tarjeta de producto
│   ├── CartItem.tsx           # Item del carrito
│   ├── Layout.tsx             # Wrapper del layout
│   └── ui/                    # Componentes UI base
│       ├── Button.tsx
│       ├── Input.tsx
│       ├── Modal.tsx
│       └── Badge.tsx
├── contexts/                  # Context API
│   ├── AuthContext.tsx        # Autenticación
│   └── CartContext.tsx        # Carrito de compras
├── services/                  # Servicios API
│   ├── api.ts                 # Cliente HTTP base
│   ├── authService.ts         # Autenticación
│   ├── productoService.ts     # Productos
│   ├── categoriaService.ts    # Categorías
│   └── ventaService.ts        # Ventas/Compras
├── types/                     # TypeScript types
│   ├── producto.ts
│   ├── categoria.ts
│   ├── cliente.ts
│   └── venta.ts
├── lib/                       # Utilidades
│   ├── utils.ts               # Funciones helper
│   └── constants.ts           # Constantes
├── public/                    # Archivos estáticos
│   ├── images/
│   └── favicon.ico
├── .env.local                 # Variables de entorno (local)
├── .env.production            # Variables de entorno (prod)
├── next.config.js             # Configuración de Next.js
├── tailwind.config.js         # Configuración de Tailwind
├── tsconfig.json              # Configuración de TypeScript
├── package.json               # Dependencias
└── Dockerfile                 # Contenedor Docker
```

---

## 🎨 Funcionalidades

### 🏠 Página Principal
- Hero section con llamada a la acción
- Categorías destacadas
- Productos más vendidos
- Testimonios de clientes

### 📦 Catálogo de Productos
- ✅ Listado de todos los productos
- ✅ Filtro por categoría
- ✅ Búsqueda por nombre
- ✅ Ordenamiento (precio, nombre, stock)
- ✅ Vista de tarjetas responsive
- ✅ Paginación
- ✅ Indicadores de stock

### 🛒 Carrito de Compras
- ✅ Agregar/remover productos
- ✅ Ajustar cantidades
- ✅ Ver subtotal y total
- ✅ Persistencia en localStorage
- ✅ Badge con cantidad de items
- ✅ Validación de stock

### 💳 Checkout
- ✅ Formulario de datos del cliente
- ✅ Selección de método de pago
- ✅ Confirmación de dirección
- ✅ Resumen de la orden
- ✅ Procesamiento del pedido
- ✅ Confirmación por email

### 📝 Mis Compras
- ✅ Historial de compras del cliente
- ✅ Detalles de cada compra
- ✅ Estado de la orden
- ✅ Descargar recibo PDF

### 🔐 Autenticación
- ✅ Registro de nuevos clientes
- ✅ Inicio de sesión
- ✅ Cierre de sesión
- ✅ Persistencia de sesión (localStorage)
- ✅ Rutas protegidas
- ✅ Perfil de usuario

---

## 🚀 Inicio Rápido

### Desarrollo Local

```bash
# Instalar dependencias
npm install

# Configurar variables de entorno
cp .env.example .env.local

# Iniciar servidor de desarrollo
npm run dev

# Abrir en navegador
http://localhost:3000
```

### Producción

```bash
# Build de producción
npm run build

# Iniciar servidor
npm start
```

---

## 🔧 Configuración

### Variables de Entorno

**`.env.local`** (desarrollo)
```env
NEXT_PUBLIC_API_URL=http://localhost:5090
```

**`.env.production`** (producción)
```env
NEXT_PUBLIC_API_URL=https://api.firmeza.com
```

### next.config.js

```javascript
module.exports = {
  output: 'standalone',
  images: {
    domains: ['localhost', 'api.firmeza.com'],
  },
  env: {
    NEXT_PUBLIC_API_URL: process.env.NEXT_PUBLIC_API_URL,
  },
}
```

---

## 🐳 Docker

### Build

```bash
docker build -t firmeza-client .
```

### Run

```bash
docker run -d \
  -p 3000:3000 \
  -e NEXT_PUBLIC_API_URL=http://localhost:5090 \
  --name firmeza-client \
  firmeza-client
```

### Con Docker Compose

```bash
docker-compose up -d client
```

---

## 📱 Rutas

| Ruta | Descripción | Autenticación |
|------|-------------|---------------|
| `/` | Página de inicio | No |
| `/productos` | Catálogo de productos | No |
| `/productos/[id]` | Detalle de producto | No |
| `/carrito` | Carrito de compras | No |
| `/checkout` | Proceso de compra | Sí |
| `/mis-compras` | Historial de compras | Sí |
| `/login` | Iniciar sesión | No |
| `/registro` | Registro de usuario | No |
| `/perfil` | Perfil del usuario | Sí |

---

## 🎨 Componentes Principales

### ProductCard

Tarjeta de producto con imagen, nombre, precio y botón de agregar al carrito.

```tsx
<ProductCard
  producto={producto}
  onAddToCart={handleAddToCart}
/>
```

### CartContext

Context para gestionar el estado global del carrito.

```tsx
const { cart, addToCart, removeFromCart, updateQuantity } = useCart();
```

### AuthContext

Context para gestionar la autenticación del usuario.

```tsx
const { user, login, logout, isAuthenticated } = useAuth();
```

---

## 🔐 Autenticación

### Flujo de Login

1. Usuario ingresa email y contraseña
2. Se envía petición a `/api/Auth/login`
3. API devuelve token JWT
4. Token se guarda en localStorage
5. Usuario es redirigido al dashboard

### Uso del Token

```typescript
// En cada petición protegida
const token = localStorage.getItem('token');
fetch(url, {
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json',
  }
});
```

---

## 🛒 Carrito de Compras

### Estructura de Datos

```typescript
interface CartItem {
  producto: Producto;
  cantidad: number;
}

interface Cart {
  items: CartItem[];
  total: number;
}
```

### Acciones Disponibles

```typescript
// Agregar producto
addToCart(producto: Producto, cantidad: number)

// Remover producto
removeFromCart(productoId: number)

// Actualizar cantidad
updateQuantity(productoId: number, cantidad: number)

// Limpiar carrito
clearCart()

// Obtener total
getTotal(): number
```

---

## 📊 TypeScript Types

### Producto

```typescript
export interface Producto {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  stock: number;
  categoriaId: number;
  categoriaNombre: string;
  activo: boolean;
  imagenUrl?: string;
}
```

### Cliente

```typescript
export interface Cliente {
  id: number;
  nombre: string;
  apellido: string;
  email: string;
  telefono?: string;
  direccion?: string;
  ciudad?: string;
  codigoPostal?: string;
}
```

### Venta

```typescript
export interface Venta {
  id: number;
  clienteId: number;
  fecha: Date;
  total: number;
  metodoPago: string;
  detalles: DetalleVenta[];
}

export interface DetalleVenta {
  productoId: number;
  productoNombre: string;
  cantidad: number;
  precioUnitario: number;
  subtotal: number;
}
```

---

## 🎨 Estilos

### Tailwind CSS

El proyecto usa **Tailwind CSS** para todos los estilos:

```tsx
<div className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
  <h2 className="text-2xl font-bold text-gray-800 mb-4">
    {producto.nombre}
  </h2>
  <p className="text-green-600 text-xl font-semibold">
    ${producto.precio.toLocaleString()}
  </p>
</div>
```

### Paleta de Colores

- **Primary:** Blue-600 (`#2563eb`)
- **Secondary:** Green-600 (`#16a34a`)
- **Accent:** Orange-500 (`#f97316`)
- **Text:** Gray-800 (`#1f2937`)
- **Background:** Gray-50 (`#f9fafb`)

---

## 📱 Responsive Design

- **Mobile First:** Diseño optimizado para móviles
- **Breakpoints:**
  - `sm`: 640px
  - `md`: 768px
  - `lg`: 1024px
  - `xl`: 1280px
  - `2xl`: 1536px

---

## 🧪 Testing

### E2E Testing (próximamente)

```bash
npm run test:e2e
```

### Unit Testing (próximamente)

```bash
npm run test
```

---

## 🚧 Roadmap

- [ ] Imágenes de productos
- [ ] Wishlist / Lista de deseos
- [ ] Comparar productos
- [ ] Reseñas y calificaciones
- [ ] Chat de soporte
- [ ] Notificaciones push
- [ ] Modo oscuro
- [ ] PWA (Progressive Web App)
- [ ] Múltiples idiomas (i18n)

---

## 📦 Dependencias Principales

```json
{
  "next": "^14.0.0",
  "react": "^18.2.0",
  "typescript": "^5.0.0",
  "tailwindcss": "^3.3.0",
  "@headlessui/react": "^1.7.0",
  "@heroicons/react": "^2.0.0"
}
```

---

## 🤝 Contribuir

Ver [CONTRIBUTING.md](../CONTRIBUTING.md) en la raíz del proyecto.

---

## 📄 Licencia

Ver [LICENSE](../LICENSE) en la raíz del proyecto.

