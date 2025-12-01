'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { authService } from '@/services/api';

export default function RegistroPage() {
  const router = useRouter();
  const [formData, setFormData] = useState({
    nombre: '',
    apellido: '',
    email: '',
    telefono: '',
    direccion: '',
    password: '',
    confirmPassword: '',
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    // Validaciones básicas
    if (formData.password !== formData.confirmPassword) {
      setError('Las contraseñas no coinciden');
      return;
    }

    if (formData.password.length < 6) {
      setError('La contraseña debe tener al menos 6 caracteres');
      return;
    }

    // Validar que tenga al menos una mayúscula
    if (!/[A-Z]/.test(formData.password)) {
      setError('La contraseña debe contener al menos una letra mayúscula (A-Z)');
      return;
    }

    // Validar que tenga al menos una minúscula
    if (!/[a-z]/.test(formData.password)) {
      setError('La contraseña debe contener al menos una letra minúscula (a-z)');
      return;
    }

    // Validar que tenga al menos un número
    if (!/[0-9]/.test(formData.password)) {
      setError('La contraseña debe contener al menos un número (0-9)');
      return;
    }

    setLoading(true);

    try {
      const response = await authService.register(formData);
      
      // Guardar token y datos del usuario
      localStorage.setItem('token', response.token);
      localStorage.setItem('user', JSON.stringify(response));
      
      // Redirigir a la tienda de clientes
      router.push('/clientes/tienda');
    } catch (err: any) {
      console.error('Error de registro completo:', err);
      console.error('Error response:', err.response);
      console.error('Error data:', err.response?.data);
      
      // Si no hay respuesta del servidor
      if (!err.response) {
        setError('No se puede conectar con el servidor. Verifica que la API esté corriendo');
        return;
      }
      
      // Extraer el mensaje de error del backend
      let errorMessage = 'Error al registrar usuario. Por favor intenta nuevamente.';
      
      if (err.response?.data) {
        const data = err.response.data;
        
        // Si es un string directo
        if (typeof data === 'string') {
          errorMessage = data;
        }
        // Si tiene una propiedad message
        else if (data.message) {
          errorMessage = data.message;
        }
        // Si tiene errores de validación de ModelState
        else if (data.errors) {
          const errors = Object.values(data.errors).flat();
          errorMessage = errors.join(', ');
        }
        // Si es un objeto, convertirlo a string legible
        else if (typeof data === 'object') {
          errorMessage = JSON.stringify(data);
        }
      }
      
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-100 via-gray-100 to-orange-50 flex items-center justify-center p-4 py-12">
      <div className="max-w-2xl w-full">
        {/* Logo/Header Industrial */}
        <div className="text-center mb-8">
          <div className="text-7xl mb-4">🏗️</div>
          <h1 className="text-5xl font-bold bg-gradient-to-r from-slate-800 via-orange-600 to-slate-800 bg-clip-text text-transparent mb-2">
            FIRMEZA
          </h1>
          <p className="text-slate-600 font-semibold text-lg">Únete a Nuestra Red de Clientes</p>
          <div className="flex justify-center gap-2 mt-3">
            <span className="bg-orange-100 text-orange-700 px-3 py-1 rounded-full text-xs font-bold">
              ⚡ Registro Rápido
            </span>
            <span className="bg-blue-100 text-blue-700 px-3 py-1 rounded-full text-xs font-bold">
              ✓ Acceso Inmediato
            </span>
          </div>
        </div>

        {/* Registro Card Industrial */}
        <div className="bg-white rounded-2xl shadow-2xl p-8 border-t-4 border-orange-600">
          <div className="flex items-center justify-center mb-6">
            <div className="text-3xl mr-3">📋</div>
            <h2 className="text-3xl font-bold text-slate-800">
              Nueva Cuenta
            </h2>
          </div>
          <p className="text-slate-600 text-center mb-8 font-medium">
            Completa tus datos para comenzar a operar con nosotros
          </p>

          {error && (
            <div className="mb-6 p-4 bg-red-50 border-l-4 border-red-600 text-red-800 rounded-r-lg">
              <div className="flex items-start">
                <span className="text-xl mr-2 flex-shrink-0">⚠️</span>
                <span className="text-sm font-medium">{error}</span>
              </div>
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-5">
            {/* Nombre y Apellido */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
              <div>
                <label htmlFor="nombre" className="block text-sm font-bold text-slate-700 mb-2">
                  👤 Nombre <span className="text-red-600">*</span>
                </label>
                <input
                  id="nombre"
                  name="nombre"
                  type="text"
                  value={formData.nombre}
                  onChange={handleChange}
                  required
                  className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition text-gray-900 bg-white placeholder-slate-400 font-medium"
                  placeholder="Juan"
                />
              </div>

              <div>
                <label htmlFor="apellido" className="block text-sm font-bold text-slate-700 mb-2">
                  👤 Apellido <span className="text-red-600">*</span>
                </label>
                <input
                  id="apellido"
                  name="apellido"
                  type="text"
                  value={formData.apellido}
                  onChange={handleChange}
                  required
                  className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition text-gray-900 bg-white placeholder-slate-400 font-medium"
                  placeholder="Pérez"
                />
              </div>
            </div>

            {/* Email */}
            <div>
              <label htmlFor="email" className="block text-sm font-bold text-slate-700 mb-2">
                📧 Correo Electrónico <span className="text-red-600">*</span>
              </label>
              <input
                id="email"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                required
                className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition text-gray-900 bg-white placeholder-slate-400 font-medium"
                placeholder="tu@empresa.com"
              />
            </div>

            {/* Teléfono */}
            <div>
              <label htmlFor="telefono" className="block text-sm font-bold text-slate-700 mb-2">
                📱 Teléfono <span className="text-slate-400 text-xs font-normal">(opcional)</span>
              </label>
              <input
                id="telefono"
                name="telefono"
                type="tel"
                value={formData.telefono}
                onChange={handleChange}
                className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition text-gray-900 bg-white placeholder-slate-400 font-medium"
                placeholder="+57 300 123 4567"
              />
            </div>

            {/* Dirección */}
            <div>
              <label htmlFor="direccion" className="block text-sm font-bold text-slate-700 mb-2">
                📍 Dirección <span className="text-red-600">*</span>
              </label>
              <input
                id="direccion"
                name="direccion"
                type="text"
                value={formData.direccion}
                onChange={handleChange}
                required
                className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition text-gray-900 bg-white placeholder-slate-400 font-medium"
                placeholder="Calle 123 #45-67, Bogotá"
              />
            </div>

            {/* Contraseña */}
            <div>
              <label htmlFor="password" className="block text-sm font-bold text-slate-700 mb-2">
                🔑 Contraseña <span className="text-red-600">*</span>
              </label>
              <input
                id="password"
                name="password"
                type="password"
                value={formData.password}
                onChange={handleChange}
                required
                minLength={6}
                className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition text-gray-900 bg-white placeholder-slate-400 font-medium"
                placeholder="MiPassword123"
              />
              <div className="mt-3 p-4 bg-slate-50 rounded-lg border-l-4 border-orange-500">
                <p className="font-bold text-xs text-slate-700 mb-2">🔒 Requisitos de seguridad:</p>
                <ul className="text-xs text-slate-600 space-y-1">
                  <li className="flex items-center"><span className="text-orange-600 mr-2">✓</span> Mínimo 6 caracteres</li>
                  <li className="flex items-center"><span className="text-orange-600 mr-2">✓</span> Una letra mayúscula (A-Z)</li>
                  <li className="flex items-center"><span className="text-orange-600 mr-2">✓</span> Una letra minúscula (a-z)</li>
                  <li className="flex items-center"><span className="text-orange-600 mr-2">✓</span> Un número (0-9)</li>
                </ul>
              </div>
            </div>

            {/* Confirmar Contraseña */}
            <div>
              <label htmlFor="confirmPassword" className="block text-sm font-bold text-slate-700 mb-2">
                🔒 Confirmar Contraseña <span className="text-red-600">*</span>
              </label>
              <input
                id="confirmPassword"
                name="confirmPassword"
                type="password"
                value={formData.confirmPassword}
                onChange={handleChange}
                required
                minLength={6}
                className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 outline-none transition text-gray-900 bg-white placeholder-slate-400 font-medium"
                placeholder="••••••••"
              />
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white font-bold py-4 px-4 rounded-lg transition duration-200 disabled:opacity-50 disabled:cursor-not-allowed shadow-lg hover:shadow-xl text-lg mt-6"
            >
              {loading ? (
                <span className="flex items-center justify-center">
                  <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                  </svg>
                  Creando tu cuenta...
                </span>
              ) : (
                '🚀 Crear Cuenta de Cliente'
              )}
            </button>
          </form>

          {/* Nota de privacidad */}
          <div className="mt-6 p-4 bg-gradient-to-r from-slate-700 to-slate-800 rounded-lg border-l-4 border-yellow-500">
            <div className="flex items-start">
              <span className="text-xl mr-2 flex-shrink-0">🛡️</span>
              <p className="text-xs text-gray-200 font-medium">
                Al registrarte, confirmas que aceptas nuestros <span className="text-yellow-400 font-bold">términos de servicio</span> y <span className="text-yellow-400 font-bold">política de privacidad</span>. Tus datos están protegidos y seguros.
              </p>
            </div>
          </div>
        </div>

        {/* Link a Login - Diseño Industrial */}
        <div className="mt-8 text-center space-y-4">
          <div className="bg-white/80 backdrop-blur-sm rounded-xl p-6 border-2 border-slate-300 shadow-lg">
            <p className="text-slate-700 font-semibold mb-3">
              ¿Ya tienes una cuenta?
            </p>
            <Link 
              href="/login" 
              className="inline-block bg-slate-700 hover:bg-slate-800 text-white font-bold py-3 px-8 rounded-lg transition-colors shadow-md hover:shadow-lg"
            >
              Iniciar Sesión
            </Link>
          </div>
          <Link 
            href="/" 
            className="inline-block text-slate-700 hover:text-orange-600 font-bold transition-colors"
          >
            ← Volver al Inicio
          </Link>
        </div>
      </div>
    </div>
  );
}

