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
      router.push('/cliente/tienda');
    } catch (err: any) {
      console.error('Error de registro completo:', err);
      console.error('Error response:', err.response);
      console.error('Error data:', err.response?.data);
      
      // Si no hay respuesta del servidor
      if (!err.response) {
        setError('No se puede conectar con el servidor. Verifica que la API esté corriendo en http://localhost:5000');
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
    <div className="min-h-screen bg-gradient-to-br from-green-50 to-emerald-100 flex items-center justify-center p-4">
      <div className="max-w-lg w-full">
        {/* Logo/Header */}
        <div className="text-center mb-8">
          <div className="text-5xl mb-3">🛍️</div>
          <h1 className="text-4xl font-bold text-gray-900 mb-2">Firmeza</h1>
          <p className="text-gray-600">Crea tu cuenta de cliente</p>
        </div>

        {/* Registro Card */}
        <div className="bg-white rounded-2xl shadow-xl p-8">
          <h2 className="text-2xl font-semibold text-gray-900 mb-2 text-center">
            Registro
          </h2>
          <p className="text-gray-500 text-sm text-center mb-6">
            Completa el formulario para crear tu cuenta
          </p>

          {error && (
            <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-700 rounded-lg text-sm">
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-4">
            {/* Nombre y Apellido */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label htmlFor="nombre" className="block text-sm font-medium text-gray-700 mb-1">
                  Nombre <span className="text-red-500">*</span>
                </label>
                <input
                  id="nombre"
                  name="nombre"
                  type="text"
                  value={formData.nombre}
                  onChange={handleChange}
                  required
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent outline-none transition text-gray-900 bg-white placeholder-gray-400"
                  placeholder="Juan"
                />
              </div>

              <div>
                <label htmlFor="apellido" className="block text-sm font-medium text-gray-700 mb-1">
                  Apellido <span className="text-red-500">*</span>
                </label>
                <input
                  id="apellido"
                  name="apellido"
                  type="text"
                  value={formData.apellido}
                  onChange={handleChange}
                  required
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent outline-none transition text-gray-900 bg-white placeholder-gray-400"
                  placeholder="Pérez"
                />
              </div>
            </div>

            {/* Email */}
            <div>
              <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-1">
                Email <span className="text-red-500">*</span>
              </label>
              <input
                id="email"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent outline-none transition text-gray-900 bg-white placeholder-gray-400"
                placeholder="tu@email.com"
              />
            </div>

            {/* Teléfono */}
            <div>
              <label htmlFor="telefono" className="block text-sm font-medium text-gray-700 mb-1">
                Teléfono <span className="text-gray-400 text-xs">(opcional)</span>
              </label>
              <input
                id="telefono"
                name="telefono"
                type="tel"
                value={formData.telefono}
                onChange={handleChange}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent outline-none transition text-gray-900 bg-white placeholder-gray-400"
                placeholder="+57 300 123 4567"
              />
            </div>

            {/* Contraseña */}
            <div>
              <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-1">
                Contraseña <span className="text-red-500">*</span>
              </label>
              <input
                id="password"
                name="password"
                type="password"
                value={formData.password}
                onChange={handleChange}
                required
                minLength={6}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent outline-none transition text-gray-900 bg-white placeholder-gray-400"
                placeholder="Ejemplo: MiPassword123"
              />
              <div className="mt-1 text-xs text-gray-500">
                <p className="font-medium mb-1">La contraseña debe contener:</p>
                <ul className="list-disc list-inside space-y-0.5">
                  <li>Mínimo 6 caracteres</li>
                  <li>Al menos una letra mayúscula (A-Z)</li>
                  <li>Al menos una letra minúscula (a-z)</li>
                  <li>Al menos un número (0-9)</li>
                </ul>
              </div>
            </div>

            {/* Confirmar Contraseña */}
            <div>
              <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-700 mb-1">
                Confirmar Contraseña <span className="text-red-500">*</span>
              </label>
              <input
                id="confirmPassword"
                name="confirmPassword"
                type="password"
                value={formData.confirmPassword}
                onChange={handleChange}
                required
                minLength={6}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent outline-none transition text-gray-900 bg-white placeholder-gray-400"
                placeholder="••••••••"
              />
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full bg-green-600 hover:bg-green-700 text-white font-semibold py-3 px-4 rounded-lg transition duration-200 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {loading ? 'Creando cuenta...' : 'Crear Cuenta'}
            </button>
          </form>

          {/* Nota de privacidad */}
          <div className="mt-4 p-3 bg-gray-50 rounded-lg">
            <p className="text-xs text-gray-600 text-center">
              Al registrarte, aceptas nuestros términos de servicio y política de privacidad
            </p>
          </div>
        </div>

        {/* Link a Login */}
        <div className="mt-6 text-center space-y-2">
          <p className="text-sm text-gray-600">
            ¿Ya tienes una cuenta?{' '}
            <Link href="/login" className="text-green-600 hover:text-green-700 font-semibold">
              Iniciar Sesión
            </Link>
          </p>
          <Link href="/" className="block text-sm text-gray-500 hover:text-gray-700">
            ← Volver al inicio
          </Link>
        </div>
      </div>
    </div>
  );
}

