'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';

export default function Home() {
  const router = useRouter();
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    // Verificar si hay token pero NO redirigir automáticamente
    const token = localStorage.getItem('token');
    setIsAuthenticated(!!token);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setIsAuthenticated(false);
    router.refresh();
  };

  return (
    <div className="min-h-screen bg-gradient-to-b from-primary-50 to-white">
      {/* Header con opción de logout si está autenticado */}
      {isAuthenticated && (
        <div className="bg-white shadow-sm border-b border-gray-200">
          <div className="container mx-auto px-4 py-3 flex justify-between items-center">
            <span className="text-sm text-gray-600">Ya has iniciado sesión</span>
            <button
              onClick={handleLogout}
              className="text-sm text-red-600 hover:text-red-700 font-medium"
            >
              Cerrar Sesión
            </button>
          </div>
        </div>
      )}

      <div className="container mx-auto px-4 py-16">
        <div className="text-center mb-12">
          <h1 className="text-5xl font-bold text-gray-900 mb-4">
            🛍️ Firmeza
          </h1>
          <p className="text-xl text-gray-600 mb-2">
            Sistema de Gestión de Ventas
          </p>
          <p className="text-gray-500">
            Clientes, Productos y Ventas - Todo en un solo lugar
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 max-w-4xl mx-auto mb-12">
          {/* Card Admin */}
          <Link href={isAuthenticated ? "/admin" : "/login"} className="group">
            <div className="bg-gradient-to-br from-blue-500 to-blue-600 p-8 rounded-lg shadow-lg hover:shadow-2xl transition-all duration-300 text-white">
              <div className="text-5xl mb-4">👨‍💼</div>
              <h2 className="text-2xl font-bold mb-2">
                Panel de Administración
              </h2>
              <p className="text-blue-100 mb-4">
                Gestiona clientes, productos, ventas y más
              </p>
              <div className="bg-white/20 px-4 py-2 rounded inline-block text-sm">
                {isAuthenticated ? 'Ir al panel →' : 'Requiere autenticación →'}
              </div>
            </div>
          </Link>

          {/* Card Cliente */}
          <Link href={isAuthenticated ? "/cliente/tienda" : "/login"} className="group">
            <div className="bg-gradient-to-br from-green-500 to-green-600 p-8 rounded-lg shadow-lg hover:shadow-2xl transition-all duration-300 text-white">
              <div className="text-5xl mb-4">🛒</div>
              <h2 className="text-2xl font-bold mb-2">
                Portal de Cliente
              </h2>
              <p className="text-green-100 mb-4">
                Explora productos, realiza compras y consulta tu historial
              </p>
              <div className="bg-white/20 px-4 py-2 rounded inline-block text-sm">
                {isAuthenticated ? 'Ir a la tienda →' : 'Iniciar sesión →'}
              </div>
            </div>
          </Link>
        </div>

        {/* Features */}
        <div className="max-w-6xl mx-auto">
          <h3 className="text-2xl font-bold text-center text-gray-900 mb-8">
            Características Principales
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="bg-white p-6 rounded-lg shadow">
              <div className="text-4xl mb-4">👥</div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">
                Gestión de Clientes
              </h3>
              <p className="text-gray-600">
                Administra tu base de clientes con información detallada
              </p>
            </div>

            <div className="bg-white p-6 rounded-lg shadow">
              <div className="text-4xl mb-4">📦</div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">
                Catálogo de Productos
              </h3>
              <p className="text-gray-600">
                Control completo de inventario y precios
              </p>
            </div>

            <div className="bg-white p-6 rounded-lg shadow">
              <div className="text-4xl mb-4">💰</div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">
                Sistema de Ventas
              </h3>
              <p className="text-gray-600">
                Registra y consulta todas tus transacciones
              </p>
            </div>
          </div>
        </div>

        <div className="mt-16 text-center">
          <div className="bg-primary-50 rounded-lg p-8 max-w-3xl mx-auto border border-primary-100">
            <h3 className="text-2xl font-semibold text-gray-900 mb-4">
              🚀 Stack Tecnológico
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 text-left text-gray-700">
              <div>
                <p className="mb-2">✅ <strong>Frontend:</strong> Next.js 14 + TypeScript</p>
                <p className="mb-2">✅ <strong>Backend:</strong> ASP.NET Core API</p>
                <p className="mb-2">✅ <strong>Styling:</strong> Tailwind CSS</p>
              </div>
              <div>
                <p className="mb-2">✅ <strong>Autenticación:</strong> JWT</p>
                <p className="mb-2">✅ <strong>Base de Datos:</strong> PostgreSQL</p>
                <p className="mb-2">✅ <strong>API Docs:</strong> Swagger</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Footer */}
      <footer className="bg-gray-900 text-white mt-16">
        <div className="container mx-auto px-4 py-8">
          <div className="text-center">
            <p className="text-gray-400">
              © 2025 Firmeza. Sistema de Gestión de Ventas.
            </p>
          </div>
        </div>
      </footer>
    </div>
  );
}

