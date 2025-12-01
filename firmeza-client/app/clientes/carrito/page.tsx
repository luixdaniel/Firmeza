'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { ventasService } from '@/services/api';
import { ShoppingCart, Trash2, Plus, Minus, ArrowLeft, CreditCard } from 'lucide-react';

interface CartItem {
  productoId: number;
  productoNombre: string;
  cantidad: number;
  precioUnitario: number;
}

const IVA_PORCENTAJE = 0.16; // 16% de IVA

export default function CarritoPage() {
  const router = useRouter();
  const [cart, setCart] = useState<CartItem[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [metodoPago, setMetodoPago] = useState('Efectivo');

  useEffect(() => {
    loadCart();
  }, []);

  const loadCart = () => {
    const cartData = JSON.parse(localStorage.getItem('cart') || '[]');
    setCart(cartData);
  };

  const updateQuantity = (productoId: number, delta: number) => {
    const updatedCart = cart.map((item) => {
      if (item.productoId === productoId) {
        const newQuantity = item.cantidad + delta;
        return { ...item, cantidad: Math.max(1, newQuantity) };
      }
      return item;
    });
    setCart(updatedCart);
    localStorage.setItem('cart', JSON.stringify(updatedCart));
    window.dispatchEvent(new Event('cartUpdated'));
  };

  const removeItem = (productoId: number) => {
    const updatedCart = cart.filter((item) => item.productoId !== productoId);
    setCart(updatedCart);
    localStorage.setItem('cart', JSON.stringify(updatedCart));
    window.dispatchEvent(new Event('cartUpdated'));
  };

  const clearCart = () => {
    if (confirm('¿Estás seguro de que deseas vaciar el carrito?')) {
      setCart([]);
      localStorage.removeItem('cart');
      window.dispatchEvent(new Event('cartUpdated'));
    }
  };

  const calculateSubtotal = () => {
    return cart.reduce((total, item) => total + item.cantidad * item.precioUnitario, 0);
  };

  const calculateIVA = () => {
    return calculateSubtotal() * IVA_PORCENTAJE;
  };

  const calculateTotal = () => {
    return calculateSubtotal() + calculateIVA();
  };

  const handleCheckout = async () => {
    if (cart.length === 0) {
      alert('El carrito está vacío');
      return;
    }

    try {
      setLoading(true);
      setError('');

      // Verificar que el usuario esté autenticado
      const token = localStorage.getItem('token');
      if (!token) {
        router.push('/login');
        return;
      }

      // Crear la venta
      const ventaData = {
        metodoPago: metodoPago,
        detalles: cart.map((item) => ({
          productoId: item.productoId,
          cantidad: item.cantidad,
          precioUnitario: item.precioUnitario,
        })),
      };

      await ventasService.create(ventaData);

      // Limpiar el carrito
      setCart([]);
      localStorage.removeItem('cart');
      window.dispatchEvent(new Event('cartUpdated'));

      // Mostrar mensaje de éxito
      alert('¡Compra realizada exitosamente! El comprobante será enviado a tu correo electrónico.');
      
      router.push('/clientes/mis-compras');
    } catch (err: any) {
      console.error('Error al procesar la compra:', err);
      console.error('Respuesta del servidor:', err.response?.data);

      let errorMessage = 'Error al procesar la compra. ';

      if (err.response?.data?.message) {
        errorMessage += err.response.data.message;
      } else if (err.response?.data?.error) {
        errorMessage += err.response.data.error;
      } else if (err.response?.status === 401) {
        errorMessage = 'No estás autenticado. Por favor, inicia sesión nuevamente.';
        setTimeout(() => router.push('/login'), 2000);
      } else if (err.response?.status === 400) {
        errorMessage += 'Datos inválidos. Verifica tu carrito e intenta nuevamente.';
      } else {
        errorMessage += 'Intenta nuevamente.';
      }

      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
      <div className="max-w-6xl mx-auto">
        {/* Header Industrial */}
        <div className="mb-8 bg-gradient-to-r from-slate-800 to-slate-700 rounded-xl p-6 border-b-4 border-orange-600 shadow-xl">
          <button
              onClick={() => router.push('/clientes/tienda')}
              className="flex items-center text-white hover:text-orange-400 mb-4 font-bold transition-colors"
          >
            <ArrowLeft className="h-6 w-6 mr-2" />
            ← Seguir Comprando
          </button>
          <div className="flex items-center gap-4">
            <div className="text-5xl">🛒</div>
            <div>
              <h1 className="text-4xl font-bold text-white mb-2">Carrito de Compras</h1>
              <p className="text-orange-300 font-bold text-lg">
                {cart.length} {cart.length === 1 ? 'producto' : 'productos'} en tu pedido
              </p>
            </div>
          </div>
        </div>

        {error && (
            <div className="mb-6 bg-gradient-to-r from-red-50 to-orange-50 border-l-4 border-red-600 rounded-r-xl p-5 shadow-lg">
              <div className="flex items-start gap-3">
                <span className="text-3xl">⚠️</span>
                <div>
                  <h3 className="font-bold text-red-800 mb-1">Error al Procesar</h3>
                  <p className="text-red-700 font-medium">{error}</p>
                </div>
              </div>
            </div>
        )}

        {cart.length === 0 ? (
            <div className="bg-gradient-to-br from-slate-50 to-orange-50 rounded-2xl shadow-xl p-16 text-center border-4 border-orange-200">
              <div className="text-8xl mb-6">🛒</div>
              <h2 className="text-3xl font-bold text-slate-800 mb-3">Tu Carrito está Vacío</h2>
              <p className="text-slate-600 font-medium mb-8 text-lg">Agrega materiales y equipos desde nuestro catálogo</p>
              <button
                  onClick={() => router.push('/clientes/tienda')}
                  className="px-8 py-4 bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white rounded-lg transition-all font-bold text-lg shadow-lg hover:shadow-xl"
              >
                🏪 Ir al Catálogo
              </button>
            </div>
        ) : (
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
              {/* Cart items Industrial */}
              <div className="lg:col-span-2 space-y-5">
                {cart.map((item) => (
                    <div
                        key={item.productoId}
                        className="bg-white rounded-xl shadow-lg p-6 flex items-center gap-6 border-2 border-slate-200 hover:border-orange-400 transition-all"
                    >
                      {/* Product placeholder image Industrial */}
                      <div className="w-24 h-24 bg-gradient-to-br from-slate-200 via-gray-200 to-orange-100 rounded-xl flex items-center justify-center flex-shrink-0 shadow-md">
                        <div className="text-4xl">🧱</div>
                      </div>

                      {/* Product info Industrial */}
                      <div className="flex-1 min-w-0">
                        <h3 className="text-xl font-bold text-slate-800 mb-2 truncate">
                          {item.productoNombre}
                        </h3>
                        <p className="text-orange-600 font-bold text-lg">
                          ${item.precioUnitario.toLocaleString('es-CO')} <span className="text-sm text-slate-500">c/u</span>
                        </p>
                      </div>

                      {/* Quantity controls Industrial */}
                      <div className="flex items-center bg-slate-100 rounded-lg p-2 gap-3 border-2 border-slate-300">
                        <button
                            onClick={() => updateQuantity(item.productoId, -1)}
                            className="p-2 rounded-lg bg-white border-2 border-slate-300 hover:bg-orange-50 hover:border-orange-500 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                            disabled={item.cantidad === 1}
                        >
                          <Minus className="h-5 w-5 text-slate-700" />
                        </button>
                        <span className="w-16 text-center font-bold text-xl text-slate-800">{item.cantidad}</span>
                        <button
                            onClick={() => updateQuantity(item.productoId, 1)}
                            className="p-2 rounded-lg bg-white border-2 border-slate-300 hover:bg-orange-50 hover:border-orange-500 transition-all"
                        >
                          <Plus className="h-5 w-5 text-slate-700" />
                        </button>
                      </div>

                      {/* Subtotal and remove Industrial */}
                      <div className="text-right">
                        <p className="text-2xl font-bold text-orange-600 mb-3">
                          ${(item.cantidad * item.precioUnitario).toLocaleString('es-CO')}
                        </p>
                        <button
                            onClick={() => removeItem(item.productoId)}
                            className="text-red-600 hover:text-red-700 flex items-center justify-end font-bold hover:bg-red-50 px-3 py-1 rounded-lg transition-all"
                        >
                          <Trash2 className="h-5 w-5 mr-1" />
                          <span className="text-sm">Eliminar</span>
                        </button>
                      </div>
                    </div>
                ))}

                {/* Clear cart button Industrial */}
                <button
                    onClick={clearCart}
                    className="w-full py-4 border-3 border-red-400 bg-red-50 text-red-700 rounded-xl hover:bg-red-100 transition-all font-bold text-lg shadow-md hover:shadow-lg flex items-center justify-center gap-2"
                >
                  <Trash2 className="h-6 w-6" />
                  Vaciar Carrito Completo
                </button>
              </div>

              {/* Order summary Industrial */}
              <div className="lg:col-span-1">
                <div className="bg-gradient-to-br from-white to-orange-50 rounded-xl shadow-2xl p-8 sticky top-24 border-t-4 border-orange-600">
                  <div className="flex items-center gap-3 mb-6">
                    <span className="text-4xl">📊</span>
                    <h2 className="text-2xl font-bold text-slate-800">Resumen del Pedido</h2>
                  </div>

                  <div className="space-y-4 mb-6 bg-white p-5 rounded-xl border-2 border-slate-200 shadow-inner">
                    <div className="flex justify-between text-slate-700 font-semibold text-lg">
                      <span>Subtotal:</span>
                      <span>${calculateSubtotal().toLocaleString('es-CO')}</span>
                    </div>
                    <div className="flex justify-between text-slate-700 font-semibold text-lg">
                      <span>IVA (16%):</span>
                      <span>${calculateIVA().toLocaleString('es-CO')}</span>
                    </div>
                    <div className="border-t-3 border-orange-300 pt-4 flex justify-between text-2xl font-bold text-orange-600">
                      <span>TOTAL:</span>
                      <span>${calculateTotal().toLocaleString('es-CO')}</span>
                    </div>
                  </div>

                  {/* Método de pago Industrial */}
                  <div className="mb-6">
                    <label className="block text-sm font-bold text-slate-700 mb-3 flex items-center gap-2">
                      <span className="text-xl">💳</span>
                      Método de Pago
                    </label>
                    <select
                        value={metodoPago}
                        onChange={(e) => setMetodoPago(e.target.value)}
                        className="w-full px-4 py-3 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 text-gray-900 font-bold bg-white shadow-sm"
                    >
                      <option value="Efectivo">💵 Efectivo</option>
                      <option value="Tarjeta">💳 Tarjeta de crédito/débito</option>
                      <option value="Transferencia">🏦 Transferencia bancaria</option>
                    </select>
                  </div>

                  <button
                      onClick={handleCheckout}
                      disabled={loading}
                      className="w-full flex items-center justify-center px-6 py-4 bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white rounded-lg transition-all disabled:from-gray-400 disabled:to-gray-500 disabled:cursor-not-allowed font-bold text-lg shadow-xl hover:shadow-2xl"
                  >
                    {loading ? (
                        <>
                          <div className="animate-spin rounded-full h-6 w-6 border-b-3 border-white mr-3"></div>
                          Procesando Compra...
                        </>
                    ) : (
                        <>
                          <CreditCard className="h-6 w-6 mr-3" />
                          🚀 Finalizar Compra
                        </>
                    )}
                  </button>

                  <div className="mt-6 p-4 bg-gradient-to-r from-slate-700 to-slate-800 rounded-lg border-l-4 border-yellow-500">
                    <p className="text-xs text-gray-200 text-center font-medium flex items-center justify-center gap-2">
                      <span>🛡️</span>
                      Al finalizar, aceptas nuestros términos y condiciones
                    </p>
                  </div>
                </div>
              </div>
            </div>
        )}
      </div>
  );
}