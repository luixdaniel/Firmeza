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
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-gray-50 to-orange-50">
      {/* Header Industrial */}
      {isAuthenticated && (
        <div className="bg-gradient-to-r from-slate-800 to-slate-700 shadow-lg border-b-4 border-orange-500">
          <div className="container mx-auto px-4 py-3 flex justify-between items-center">
            <span className="text-sm text-gray-300 font-medium">🏗️ Bienvenido a Firmeza</span>
            <button
              onClick={handleLogout}
              className="text-sm bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded font-medium transition-colors"
            >
              Cerrar Sesión
            </button>
          </div>
        </div>
      )}

      <div className="container mx-auto px-4 py-16">
        {/* Hero Section Industrial */}
        <div className="text-center mb-16">
          <div className="inline-block mb-6">
            <div className="text-8xl mb-4">🏗️</div>
          </div>
          <h1 className="text-6xl font-bold bg-gradient-to-r from-slate-800 via-orange-600 to-slate-800 bg-clip-text text-transparent mb-4">
            FIRMEZA
          </h1>
          <p className="text-3xl font-bold text-slate-700 mb-3">
            Insumos de Construcción y Renta de Vehículos
          </p>
          <p className="text-lg text-slate-600 mb-4 max-w-3xl mx-auto">
            Proveedor líder en materiales de construcción de calidad y renta de vehículos industriales
          </p>
          <div className="flex justify-center gap-2 text-sm text-slate-500 mb-8">
            <span className="bg-orange-100 text-orange-700 px-3 py-1 rounded-full font-medium">⚡ Entrega Rápida</span>
            <span className="bg-blue-100 text-blue-700 px-3 py-1 rounded-full font-medium">🏆 Calidad Garantizada</span>
            <span className="bg-green-100 text-green-700 px-3 py-1 rounded-full font-medium">✓ Servicio Profesional</span>
          </div>
          
          {/* CTA Principal */}
          <div className="flex justify-center gap-4 mb-8">
            {isAuthenticated ? (
              <Link 
                href="/clientes/tienda" 
                className="bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white px-10 py-5 rounded-lg text-lg font-bold shadow-xl hover:shadow-2xl transition-all duration-300 border-2 border-orange-800"
              >
                🏪 Ir a la Tienda
              </Link>
            ) : (
              <>
                <Link 
                  href="/login" 
                  className="bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white px-10 py-5 rounded-lg text-lg font-bold shadow-xl hover:shadow-2xl transition-all duration-300"
                >
                  Iniciar Sesión
                </Link>
                <Link 
                  href="/registro" 
                  className="bg-white hover:bg-gray-50 text-orange-600 px-10 py-5 rounded-lg text-lg font-bold border-3 border-orange-600 shadow-xl hover:shadow-2xl transition-all duration-300"
                >
                  Registrarse
                </Link>
              </>
            )}
          </div>
        </div>

        {/* Servicios Principales */}
        <div className="max-w-6xl mx-auto mb-16">
          <h2 className="text-4xl font-bold text-center text-slate-800 mb-12 flex items-center justify-center gap-3">
            <span className="text-orange-600">⚙️</span>
            Nuestros Servicios
            <span className="text-orange-600">⚙️</span>
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {/* Insumos de Construcción */}
            <div className="bg-gradient-to-br from-white to-orange-50 p-8 rounded-xl shadow-lg hover:shadow-2xl transition-all border-l-4 border-orange-600 hover:scale-105">
              <div className="text-6xl mb-4 text-center">🧱</div>
              <h3 className="text-2xl font-bold text-slate-800 mb-3 text-center">
                Insumos de Construcción
              </h3>
              <p className="text-slate-600 text-center mb-4">
                Cemento, arena, grava, blocks, varillas y todos los materiales para tu obra
              </p>
              <div className="text-sm text-orange-600 font-semibold text-center">
                ✓ Calidad certificada
              </div>
            </div>

            {/* Vehículos Industriales */}
            <div className="bg-gradient-to-br from-white to-blue-50 p-8 rounded-xl shadow-lg hover:shadow-2xl transition-all border-l-4 border-blue-600 hover:scale-105">
              <div className="text-6xl mb-4 text-center">🚜</div>
              <h3 className="text-2xl font-bold text-slate-800 mb-3 text-center">
                Renta de Vehículos
              </h3>
              <p className="text-slate-600 text-center mb-4">
                Maquinaria pesada, montacargas, grúas y equipo industrial para tu proyecto
              </p>
              <div className="text-sm text-blue-600 font-semibold text-center">
                ✓ Mantenimiento incluido
              </div>
            </div>

            {/* Herramientas y Equipo */}
            <div className="bg-gradient-to-br from-white to-yellow-50 p-8 rounded-xl shadow-lg hover:shadow-2xl transition-all border-l-4 border-yellow-600 hover:scale-105">
              <div className="text-6xl mb-4 text-center">🔨</div>
              <h3 className="text-2xl font-bold text-slate-800 mb-3 text-center">
                Herramientas y Equipo
              </h3>
              <p className="text-slate-600 text-center mb-4">
                Herramientas eléctricas, equipo de seguridad y accesorios profesionales
              </p>
              <div className="text-sm text-yellow-600 font-semibold text-center">
                ✓ Última tecnología
              </div>
            </div>
          </div>
        </div>

        {/* Beneficios Industriales */}
        <div className="max-w-5xl mx-auto mb-16">
          <div className="bg-gradient-to-r from-slate-800 via-slate-700 to-slate-800 rounded-2xl p-12 text-white shadow-2xl border-t-4 border-orange-500">
            <h2 className="text-4xl font-bold mb-8 text-center">
              ¿Por Qué Elegirnos?
            </h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="flex items-start bg-slate-700/50 p-4 rounded-lg">
                <div className="text-3xl mr-4 text-orange-400">🏆</div>
                <div>
                  <h3 className="font-bold mb-1 text-lg">Calidad Profesional</h3>
                  <p className="text-gray-300">Materiales certificados y equipo de primera calidad</p>
                </div>
              </div>
              <div className="flex items-start bg-slate-700/50 p-4 rounded-lg">
                <div className="text-3xl mr-4 text-orange-400">⚡</div>
                <div>
                  <h3 className="font-bold mb-1 text-lg">Entrega Inmediata</h3>
                  <p className="text-gray-300">Disponibilidad inmediata y entrega en obra</p>
                </div>
              </div>
              <div className="flex items-start bg-slate-700/50 p-4 rounded-lg">
                <div className="text-3xl mr-4 text-orange-400">👷</div>
                <div>
                  <h3 className="font-bold mb-1 text-lg">Asesoría Técnica</h3>
                  <p className="text-gray-300">Personal capacitado para asesorarte en tu proyecto</p>
                </div>
              </div>
              <div className="flex items-start bg-slate-700/50 p-4 rounded-lg">
                <div className="text-3xl mr-4 text-orange-400">💰</div>
                <div>
                  <h3 className="font-bold mb-1 text-lg">Precios Competitivos</h3>
                  <p className="text-gray-300">Mejores precios del mercado con financiamiento</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Call to Action Final */}
        {!isAuthenticated && (
          <div className="text-center bg-gradient-to-br from-orange-50 to-yellow-50 rounded-2xl p-12 max-w-4xl mx-auto border-4 border-orange-200 shadow-2xl">
            <div className="text-6xl mb-6">🏗️</div>
            <h2 className="text-4xl font-bold text-slate-800 mb-4">
              ¿Listo para tu Proyecto?
            </h2>
            <p className="text-xl text-slate-600 mb-8 max-w-2xl mx-auto">
              Únete a cientos de constructores que confían en nosotros para sus proyectos
            </p>
            <div className="flex justify-center gap-4">
              <Link 
                href="/registro" 
                className="inline-block bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white px-12 py-5 rounded-lg text-xl font-bold shadow-xl hover:shadow-2xl transition-all duration-300"
              >
                Registrarse Ahora →
              </Link>
              <Link 
                href="/login" 
                className="inline-block bg-white hover:bg-gray-50 text-orange-600 border-3 border-orange-600 px-12 py-5 rounded-lg text-xl font-bold shadow-xl hover:shadow-2xl transition-all duration-300"
              >
                Iniciar Sesión
              </Link>
            </div>
            <p className="text-sm text-slate-500 mt-6">
              ✓ Sin costo de registro | ✓ Cotización inmediata | ✓ Entrega en obra
            </p>
          </div>
        )}
      </div>

      {/* Footer Industrial */}
      <footer className="bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900 text-white mt-20 border-t-4 border-orange-600">
        <div className="container mx-auto px-4 py-12">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-8">
            <div>
              <h3 className="text-2xl font-bold mb-4 text-orange-400">🏗️ FIRMEZA</h3>
              <p className="text-gray-300">
                Tu socio confiable en construcción y equipo industrial desde hace más de 20 años.
              </p>
            </div>
            <div>
              <h4 className="font-bold mb-4 text-orange-400">Servicios</h4>
              <ul className="space-y-2 text-gray-300">
                <li>• Insumos de Construcción</li>
                <li>• Renta de Vehículos</li>
                <li>• Herramientas y Equipo</li>
                <li>• Asesoría Técnica</li>
              </ul>
            </div>
            <div>
              <h4 className="font-bold mb-4 text-orange-400">Contacto</h4>
              <ul className="space-y-2 text-gray-300">
                <li>📞 Teléfono: (123) 456-7890</li>
                <li>📧 Email: ventas@firmeza.com</li>
                <li>🕐 Lun-Sab: 7:00 AM - 6:00 PM</li>
              </ul>
            </div>
          </div>
          <div className="border-t border-slate-700 pt-8 text-center">
            <p className="text-gray-400 mb-2 font-medium">
              © 2025 Firmeza - Insumos de Construcción y Renta de Vehículos Industriales
            </p>
            <p className="text-sm text-gray-500">
              Construyendo el futuro juntos | Calidad garantizada | Servicio profesional
            </p>
          </div>
        </div>
      </footer>
    </div>
  );
}

