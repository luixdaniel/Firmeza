'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';

export default function Home() {
  const router = useRouter();
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    // Verificar si hay token
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
    <div className="min-h-screen bg-gradient-to-b from-green-50 to-white">
      {/* Header con opción de logout si está autenticado */}
      {isAuthenticated && (
        <div className="bg-white shadow-sm border-b border-gray-200">
          <div className="container mx-auto px-4 py-3 flex justify-between items-center">
            <span className="text-sm text-gray-600">Bienvenido de nuevo</span>
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
        {/* Hero Section */}
        <div className="text-center mb-16">
          <h1 className="text-6xl font-bold text-gray-900 mb-4">
            🛍️ Firmeza
          </h1>
          <p className="text-2xl text-gray-600 mb-3">
            Tu Tienda en Línea
          </p>
          <p className="text-lg text-gray-500 mb-8">
            Descubre productos de calidad, realiza tus compras y gestiona tus pedidos
          </p>
          
          {/* CTA Principal */}
          <div className="flex justify-center gap-4 mb-8">
            {isAuthenticated ? (
              <Link 
                href="/clientes/tienda" 
                className="bg-green-600 hover:bg-green-700 text-white px-8 py-4 rounded-lg text-lg font-semibold shadow-lg hover:shadow-xl transition-all duration-300"
              >
                🛒 Ir a la Tienda
              </Link>
            ) : (
              <>
                <Link 
                  href="/login" 
                  className="bg-green-600 hover:bg-green-700 text-white px-8 py-4 rounded-lg text-lg font-semibold shadow-lg hover:shadow-xl transition-all duration-300"
                >
                  Iniciar Sesión
                </Link>
                <Link 
                  href="/registro" 
                  className="bg-white hover:bg-gray-50 text-green-600 px-8 py-4 rounded-lg text-lg font-semibold border-2 border-green-600 shadow-lg hover:shadow-xl transition-all duration-300"
                >
                  Registrarse
                </Link>
              </>
            )}
          </div>
        </div>

        {/* Características del Portal de Clientes */}
        <div className="max-w-6xl mx-auto mb-16">
          <h2 className="text-3xl font-bold text-center text-gray-900 mb-12">
            ¿Qué puedes hacer en Firmeza?
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="bg-white p-8 rounded-xl shadow-lg hover:shadow-xl transition-shadow">
              <div className="text-5xl mb-4">🛒</div>
              <h3 className="text-xl font-semibold text-gray-900 mb-3">
                Explora Productos
              </h3>
              <p className="text-gray-600">
                Navega por nuestro catálogo completo de productos organizados por categorías
              </p>
            </div>

            <div className="bg-white p-8 rounded-xl shadow-lg hover:shadow-xl transition-shadow">
              <div className="text-5xl mb-4">💳</div>
              <h3 className="text-xl font-semibold text-gray-900 mb-3">
                Realiza Compras
              </h3>
              <p className="text-gray-600">
                Añade productos a tu carrito y completa tus compras de manera segura
              </p>
            </div>

            <div className="bg-white p-8 rounded-xl shadow-lg hover:shadow-xl transition-shadow">
              <div className="text-5xl mb-4">📋</div>
              <h3 className="text-xl font-semibold text-gray-900 mb-3">
                Historial de Pedidos
              </h3>
              <p className="text-gray-600">
                Consulta todas tus compras anteriores y el estado de tus pedidos
              </p>
            </div>
          </div>
        </div>

        {/* Beneficios */}
        <div className="max-w-4xl mx-auto mb-16">
          <div className="bg-gradient-to-r from-green-600 to-green-700 rounded-2xl p-12 text-white shadow-2xl">
            <h2 className="text-3xl font-bold mb-6 text-center">
              Beneficios de Comprar con Nosotros
            </h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="flex items-start">
                <div className="text-2xl mr-3">✅</div>
                <div>
                  <h3 className="font-semibold mb-1">Productos de Calidad</h3>
                  <p className="text-green-100">Selección cuidadosa de productos</p>
                </div>
              </div>
              <div className="flex items-start">
                <div className="text-2xl mr-3">✅</div>
                <div>
                  <h3 className="font-semibold mb-1">Compra Segura</h3>
                  <p className="text-green-100">Sistema de pago protegido</p>
                </div>
              </div>
              <div className="flex items-start">
                <div className="text-2xl mr-3">✅</div>
                <div>
                  <h3 className="font-semibold mb-1">Seguimiento Fácil</h3>
                  <p className="text-green-100">Revisa tus pedidos en tiempo real</p>
                </div>
              </div>
              <div className="flex items-start">
                <div className="text-2xl mr-3">✅</div>
                <div>
                  <h3 className="font-semibold mb-1">Atención al Cliente</h3>
                  <p className="text-green-100">Soporte cuando lo necesites</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Call to Action Final */}
        {!isAuthenticated && (
          <div className="text-center bg-gray-50 rounded-2xl p-12 max-w-3xl mx-auto">
            <h2 className="text-3xl font-bold text-gray-900 mb-4">
              ¿Listo para empezar?
            </h2>
            <p className="text-lg text-gray-600 mb-6">
              Crea tu cuenta o inicia sesión para comenzar a comprar
            </p>
            <div className="flex justify-center gap-4">
              <Link 
                href="/registro" 
                className="inline-block bg-green-600 hover:bg-green-700 text-white px-10 py-4 rounded-lg text-lg font-semibold shadow-lg hover:shadow-xl transition-all duration-300"
              >
                Crear Cuenta →
              </Link>
              <Link 
                href="/login" 
                className="inline-block bg-white hover:bg-gray-50 text-green-600 border-2 border-green-600 px-10 py-4 rounded-lg text-lg font-semibold shadow-lg hover:shadow-xl transition-all duration-300"
              >
                Iniciar Sesión
              </Link>
            </div>
          </div>
        )}
      </div>

      {/* Footer */}
      <footer className="bg-gray-900 text-white mt-20">
        <div className="container mx-auto px-4 py-8">
          <div className="text-center">
            <p className="text-gray-400 mb-2">
              © 2025 Firmeza - Portal de Clientes
            </p>
            <p className="text-sm text-gray-500">
              Tu tienda en línea de confianza
            </p>
          </div>
        </div>
      </footer>
    </div>
  );
}

