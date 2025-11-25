'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import {
  Users,
  Package,
  ShoppingBag,
  DollarSign,
  TrendingUp,
  ArrowRight,
} from 'lucide-react';
import { clientesService, productosService, ventasService } from '@/services/api';

export default function AdminDashboard() {
  const [stats, setStats] = useState({
    totalClientes: 0,
    totalProductos: 0,
    totalVentas: 0,
    ingresosTotales: 0,
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // El layout ya verifica la autenticación, solo cargamos los datos
    loadStats();
  }, []);

  const loadStats = async () => {
    try {
      setLoading(true);
      const [clientes, productos, ventas] = await Promise.all([
        clientesService.getAll(),
        productosService.getAll(),
        ventasService.getAll(),
      ]);

      const ingresos = ventas.reduce((total, venta) => total + venta.total, 0);

      setStats({
        totalClientes: clientes.length,
        totalProductos: productos.length,
        totalVentas: ventas.length,
        ingresosTotales: ingresos,
      });
    } catch (err) {
      console.error('Error loading stats:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center py-12">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div>
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <p className="text-gray-600 mt-1">
          Resumen general del sistema - {new Date().toLocaleDateString('es-ES', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric',
          })}
        </p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        {/* Clientes */}
        <div className="bg-gradient-to-br from-blue-500 to-blue-600 rounded-lg shadow-lg p-6 text-white">
          <div className="flex items-center justify-between mb-4">
            <Users className="h-10 w-10 opacity-80" />
            <TrendingUp className="h-6 w-6 opacity-60" />
          </div>
          <div className="text-3xl font-bold mb-1">{stats.totalClientes}</div>
          <div className="text-blue-100 text-sm mb-4">Clientes Totales</div>
          <Link
            href="/admin/clientes"
            className="flex items-center text-sm font-medium hover:underline"
          >
            Ver todos <ArrowRight className="h-4 w-4 ml-1" />
          </Link>
        </div>

        {/* Productos */}
        <div className="bg-gradient-to-br from-purple-500 to-purple-600 rounded-lg shadow-lg p-6 text-white">
          <div className="flex items-center justify-between mb-4">
            <Package className="h-10 w-10 opacity-80" />
            <TrendingUp className="h-6 w-6 opacity-60" />
          </div>
          <div className="text-3xl font-bold mb-1">{stats.totalProductos}</div>
          <div className="text-purple-100 text-sm mb-4">Productos en Catálogo</div>
          <Link
            href="/admin/productos"
            className="flex items-center text-sm font-medium hover:underline"
          >
            Ver todos <ArrowRight className="h-4 w-4 ml-1" />
          </Link>
        </div>

        {/* Ventas */}
        <div className="bg-gradient-to-br from-green-500 to-green-600 rounded-lg shadow-lg p-6 text-white">
          <div className="flex items-center justify-between mb-4">
            <ShoppingBag className="h-10 w-10 opacity-80" />
            <TrendingUp className="h-6 w-6 opacity-60" />
          </div>
          <div className="text-3xl font-bold mb-1">{stats.totalVentas}</div>
          <div className="text-green-100 text-sm mb-4">Ventas Realizadas</div>
          <Link
            href="/admin/ventas"
            className="flex items-center text-sm font-medium hover:underline"
          >
            Ver todas <ArrowRight className="h-4 w-4 ml-1" />
          </Link>
        </div>

        {/* Ingresos */}
        <div className="bg-gradient-to-br from-orange-500 to-orange-600 rounded-lg shadow-lg p-6 text-white">
          <div className="flex items-center justify-between mb-4">
            <DollarSign className="h-10 w-10 opacity-80" />
            <TrendingUp className="h-6 w-6 opacity-60" />
          </div>
          <div className="text-3xl font-bold mb-1">
            ${stats.ingresosTotales.toFixed(2)}
          </div>
          <div className="text-orange-100 text-sm mb-4">Ingresos Totales</div>
          <Link
            href="/admin/ventas"
            className="flex items-center text-sm font-medium hover:underline"
          >
            Ver detalle <ArrowRight className="h-4 w-4 ml-1" />
          </Link>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
        <div className="bg-white rounded-lg shadow p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">
            Acciones Rápidas
          </h3>
          <div className="space-y-3">
            <Link
              href="/admin/clientes"
              className="flex items-center justify-between p-4 border border-gray-200 rounded-lg hover:border-primary-500 hover:bg-primary-50 transition"
            >
              <div className="flex items-center space-x-3">
                <Users className="h-5 w-5 text-primary-600" />
                <span className="font-medium">Gestionar Clientes</span>
              </div>
              <ArrowRight className="h-5 w-5 text-gray-400" />
            </Link>

            <Link
              href="/admin/productos"
              className="flex items-center justify-between p-4 border border-gray-200 rounded-lg hover:border-primary-500 hover:bg-primary-50 transition"
            >
              <div className="flex items-center space-x-3">
                <Package className="h-5 w-5 text-primary-600" />
                <span className="font-medium">Gestionar Productos</span>
              </div>
              <ArrowRight className="h-5 w-5 text-gray-400" />
            </Link>

            <Link
              href="/admin/ventas"
              className="flex items-center justify-between p-4 border border-gray-200 rounded-lg hover:border-primary-500 hover:bg-primary-50 transition"
            >
              <div className="flex items-center space-x-3">
                <ShoppingBag className="h-5 w-5 text-primary-600" />
                <span className="font-medium">Ver Ventas</span>
              </div>
              <ArrowRight className="h-5 w-5 text-gray-400" />
            </Link>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">
            Actividad Reciente
          </h3>
          <div className="space-y-4 text-sm text-gray-600">
            <div className="flex items-start space-x-3">
              <div className="bg-green-100 p-2 rounded">
                <ShoppingBag className="h-4 w-4 text-green-600" />
              </div>
              <div>
                <div className="font-medium text-gray-900">Nueva venta registrada</div>
                <div className="text-xs text-gray-500">Hace 5 minutos</div>
              </div>
            </div>

            <div className="flex items-start space-x-3">
              <div className="bg-blue-100 p-2 rounded">
                <Users className="h-4 w-4 text-blue-600" />
              </div>
              <div>
                <div className="font-medium text-gray-900">Nuevo cliente registrado</div>
                <div className="text-xs text-gray-500">Hace 1 hora</div>
              </div>
            </div>

            <div className="flex items-start space-x-3">
              <div className="bg-purple-100 p-2 rounded">
                <Package className="h-4 w-4 text-purple-600" />
              </div>
              <div>
                <div className="font-medium text-gray-900">Producto actualizado</div>
                <div className="text-xs text-gray-500">Hace 2 horas</div>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Welcome Message */}
      <div className="bg-gradient-to-r from-primary-600 to-primary-700 rounded-lg shadow-lg p-8 text-white">
        <h2 className="text-2xl font-bold mb-2">¡Bienvenido al Panel de Administración!</h2>
        <p className="text-primary-100">
          Desde aquí puedes gestionar clientes, productos, ventas y mucho más.
        </p>
      </div>
    </div>
  );
}
