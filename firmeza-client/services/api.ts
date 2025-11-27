import { api } from '@/lib/axios';
import { LoginRequest, RegisterRequest, AuthResponse, Cliente, Venta, Producto, Categoria } from '@/types';

// Auth Services
export const authService = {
  async login(credentials: LoginRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/Auth/login', credentials);
    return response.data;
  },

  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/Auth/register', data);
    return response.data;
  },

  async getMe(): Promise<any> {
    const response = await api.get('/Auth/me');
    return response.data;
  },
};

// Clientes Services
export const clientesService = {
  async getAll(): Promise<Cliente[]> {
    const response = await api.get<Cliente[]>('/Clientes');
    return response.data;
  },

  async getById(id: number): Promise<Cliente> {
    const response = await api.get<Cliente>(`/Clientes/${id}`);
    return response.data;
  },

  async create(cliente: Partial<Cliente>): Promise<Cliente> {
    const response = await api.post<Cliente>('/Clientes', cliente);
    return response.data;
  },

  async update(id: number, cliente: Partial<Cliente>): Promise<void> {
    await api.put(`/Clientes/${id}`, cliente);
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/Clientes/${id}`);
  },
};

// Ventas Services
export const ventasService = {
  async getAll(): Promise<Venta[]> {
    const response = await api.get<Venta[]>('/Ventas');
    return response.data;
  },

  async getById(id: number): Promise<Venta> {
    const response = await api.get<Venta>(`/Ventas/${id}`);
    return response.data;
  },

  async create(venta: any): Promise<Venta> {
    const response = await api.post<Venta>('/Ventas', venta);
    return response.data;
  },
};

// Productos Services
export const productosService = {
  async getAll(): Promise<Producto[]> {
    const response = await api.get<Producto[]>('/Productos');
    return response.data;
  },

  async getById(id: number): Promise<Producto> {
    const response = await api.get<Producto>(`/Productos/${id}`);
    return response.data;
  },

  async create(producto: Partial<Producto>): Promise<Producto> {
    const response = await api.post<Producto>('/Productos', producto);
    return response.data;
  },

  async update(id: number, producto: Partial<Producto>): Promise<void> {
    await api.put(`/Productos/${id}`, producto);
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/Productos/${id}`);
  },
};

// Categorías Services
export const categoriasService = {
  async getAll(): Promise<Categoria[]> {
    const response = await api.get<Categoria[]>('/Categorias');
    return response.data;
  },
};

