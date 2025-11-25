'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { User, ShoppingBag, ShoppingCart, LogOut } from 'lucide-react';

export default function ClienteLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const router = useRouter();
  const [userName, setUserName] = useState('');
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Verificar autenticación
    const checkAuth = () => {
      const token = localStorage.getItem('token');
      
      if (!token) {
        router.push('/login');
        return;
      }

      const userStr = localStorage.getItem('user');
      if (userStr) {
        try {
          const user = JSON.parse(userStr);
          setUserName(user.email || 'Cliente');
        } catch (error) {
          console.error('Error parsing user data:', error);
          setUserName('Cliente');
        }
      }
      
      setIsLoading(false);
    };

    checkAuth();
  }, [router]);

  // Mostrar loading mientras verifica autenticación
  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Cargando...</p>
        </div>
      </div>
    );
  }

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    router.push('/login');
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow-sm border-b border-gray-200">
        <div className="container mx-auto px-4 py-4">
          <div className="flex justify-between items-center">
            <Link href="/cliente/tienda" className="flex items-center space-x-2">
              <ShoppingBag className="h-8 w-8 text-primary-600" />
              <h1 className="text-2xl font-bold text-gray-900">Firmeza</h1>
            </Link>

            <nav className="hidden md:flex space-x-6">
              <Link
                href="/cliente/tienda"
                className="flex items-center space-x-2 text-gray-600 hover:text-primary-600 transition"
              >
                <ShoppingCart className="h-5 w-5" />
                <span>Tienda</span>
              </Link>
              <Link
                href="/cliente/mis-compras"
                className="flex items-center space-x-2 text-gray-600 hover:text-primary-600 transition"
              >
                <ShoppingBag className="h-5 w-5" />
                <span>Mis Compras</span>
              </Link>
              <Link
                href="/cliente/perfil"
                className="flex items-center space-x-2 text-gray-600 hover:text-primary-600 transition"
              >
                <User className="h-5 w-5" />
                <span>Mi Perfil</span>
              </Link>
            </nav>

            <div className="flex items-center space-x-4">
              <div className="hidden md:block text-sm">
                <div className="text-gray-500">Bienvenido,</div>
                <div className="font-semibold text-gray-900">{userName}</div>
              </div>
              <button
                onClick={handleLogout}
                className="flex items-center space-x-2 text-red-600 hover:text-red-700 font-medium transition"
              >
                <LogOut className="h-5 w-5" />
                <span className="hidden md:inline">Salir</span>
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="container mx-auto px-4 py-8">
        {children}
      </main>

      {/* Footer */}
      <footer className="bg-white border-t border-gray-200 mt-12">
        <div className="container mx-auto px-4 py-6">
          <p className="text-center text-gray-600 text-sm">
            © 2025 Firmeza. Todos los derechos reservados.
          </p>
        </div>
      </footer>
    </div>
  );
}

