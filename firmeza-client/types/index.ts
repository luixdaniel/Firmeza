// Types basados en tu API
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  nombre: string;
  apellido: string;
  telefono?: string;
  documento?: string;
  direccion: string;
  ciudad?: string;
  pais?: string;
}

export interface AuthResponse {
  token: string;
  expiration: string;
  email: string;
  nombreCompleto: string;
  roles: string[];
}

export interface Cliente {
  id: number;
  nombre: string;
  apellido: string;
  nombreCompleto: string;
  email: string;
  telefono: string;
  documento: string;
  direccion: string;
  ciudad: string;
  pais: string;
  fechaRegistro: string;
  activo: boolean;
}

export interface Producto {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  stock: number;
  categoriaId: number;
  categoriaNombre: string;
  imagenUrl?: string;
  activo: boolean;
}

export interface VentaDetalle {
  id: number;
  productoId: number;
  productoNombre: string;
  cantidad: number;
  precioUnitario: number;
  subtotal: number;
}

export interface Venta {
  id: number;
  fecha: string;
  clienteId: number;
  clienteNombre: string;
  total: number;
  detalles: VentaDetalle[];
}

export interface Categoria {
  id: number;
  nombre: string;
  descripcion: string;
  activa: boolean;
}

