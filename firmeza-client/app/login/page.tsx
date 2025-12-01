'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { authService } from '@/services/api';

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await authService.login({ email, password });
      
      // Guardar token y datos del usuario
      localStorage.setItem('token', response.token);
      localStorage.setItem('user', JSON.stringify(response));
      
      // Redirigir a la tienda de clientes
      router.push('/clientes/tienda');
    } catch (err: any) {
      console.error('Error de login:', err);
      setError(err.response?.data?.message || 'Credenciales inválidas. Por favor verifica tus datos.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-100 via-gray-100 to-orange-50 flex items-center justify-center p-4">
      <div className="max-w-md w-full">
        {/* Logo/Header Industrial */}
        <div className="text-center mb-8">
          <div className="text-7xl mb-4">🏗️</div>
          <h1 className="text-5xl font-bold bg-gradient-to-r from-slate-800 via-orange-600 to-slate-800 bg-clip-text text-transparent mb-2">
            FIRMEZA
          </h1>
          <p className="text-slate-600 font-semibold">Insumos y Vehículos Industriales</p>
        </div>

        {/* Login Card Industrial */}
        <div className="bg-white rounded-2xl shadow-2xl p-8 border-t-4 border-orange-600">
          <div className="flex items-center justify-center mb-6">
            <div className="text-3xl mr-3">🔒</div>
            <h2 className="text-3xl font-bold text-slate-800">
              Acceso al Portal
            </h2>
          </div>
          <p className="text-slate-600 text-center mb-8 font-medium">
            Ingresa con tus credenciales de cliente
          </p>

          {error && (
            <div className="mb-6 p-4 bg-red-50 border-l-4 border-red-600 text-red-800 rounded-r-lg">
              <div className="flex items-start">
                <span className="text-xl mr-2">⚠️</span>
                <span className="text-sm font-medium">{error}</span>
              </div>
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label htmlFor="email" className="block text-sm font-bold text-slate-700 mb-2">
                📧 Correo Electrónico
              </label>
              <input
                id="email"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition text-gray-900 bg-white placeholder-slate-400 font-medium"
                placeholder="tu@empresa.com"
              />
            </div>

            <div>
              <label htmlFor="password" className="block text-sm font-bold text-slate-700 mb-2">
                🔑 Contraseña
              </label>
              <input
                id="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
                className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition text-gray-900 bg-white placeholder-slate-400 font-medium"
                placeholder="••••••••"
              />
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white font-bold py-4 px-4 rounded-lg transition duration-200 disabled:opacity-50 disabled:cursor-not-allowed shadow-lg hover:shadow-xl text-lg"
            >
              {loading ? (
                <span className="flex items-center justify-center">
                  <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                  </svg>
                  Verificando credenciales...
                </span>
              ) : (
                '🚀 Iniciar Sesión'
              )}
            </button>
          </form>

          {/* Credenciales de prueba - Diseño Industrial */}
          <div className="mt-8 p-5 bg-gradient-to-br from-slate-700 to-slate-800 rounded-xl border-l-4 border-yellow-500 shadow-lg">
            <div className="flex items-center mb-3">
              <span className="text-2xl mr-2">🔑</span>
              <p className="text-sm font-bold text-yellow-400">
                CREDENCIALES DE PRUEBA
              </p>
            </div>
            <div className="text-sm text-gray-200 space-y-2 bg-slate-900/50 p-3 rounded">
              <p className="flex justify-between">
                <span className="font-semibold text-yellow-400">Email:</span>
                <span className="font-mono">cliente@firmeza.com</span>
              </p>
              <p className="flex justify-between">
                <span className="font-semibold text-yellow-400">Password:</span>
                <span className="font-mono">Cliente123$</span>
              </p>
            </div>
          </div>
        </div>

        {/* Footer Links */}
        <div className="mt-8 text-center space-y-4">
          <Link 
            href="/" 
            className="inline-block text-slate-700 hover:text-orange-600 font-bold transition-colors"
          >
            ← Volver al Inicio
          </Link>
          <div className="bg-white/80 backdrop-blur-sm rounded-lg p-4 border border-slate-300">
            <p className="text-sm text-slate-600 mb-2">
              ¿No tienes cuenta?
            </p>
            <Link 
              href="/registro" 
              className="inline-block bg-slate-700 hover:bg-slate-800 text-white font-bold py-2 px-6 rounded-lg transition-colors"
            >
              Registrarse Ahora
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}

