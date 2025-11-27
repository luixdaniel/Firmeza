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

export default function CarritoPage() {
  const router = useRouter();
  const [cart, setCart] = useState<CartItem[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

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

  const calculateTotal = () => {
    return cart.reduce((total, item) => total + item.cantidad * item.precioUnitario, 0);
  };

  const handleCheckout = async () => {
    if (cart.length === 0) {
      alert('El carrito está vacío');
      return;
    }

    try {
      setLoading(true);
      setError('');

      // Obtener el clienteId del usuario actual
      const userData = localStorage.getItem('user');
      if (!userData) {
        router.push('/auth/login');
        return;
      }

      const user = JSON.parse(userData);
      
      // Crear la venta
      const ventaData = {
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

      // Redirigir a mis compras con mensaje de éxito
      alert('¡Compra realizada exitosamente!');
      router.push('/clientes/mis-compras');
    } catch (err: any) {
      console.error('Error al procesar la compra:', err);
      setError(err.response?.data?.message || 'Error al procesar la compra. Intenta nuevamente.');
    } finally {
      setLoading(false);
    }
  };

  if (cart.length === 0) {
    return (
      <div className="max-w-4xl mx-auto">
        <div className="text-center py-12">
          <ShoppingCart className="h-24 w-24 text-gray-300 mx-auto mb-4" />
          <h2 className="text-2xl font-bold text-gray-900 mb-2">Tu carrito está vacío</h2>
          <p className="text-gray-600 mb-6">Agrega productos para comenzar tu compra</p>
          <button
            onClick={() => router.push('/clientes/tienda')}
            className="inline-flex items-center px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            <ArrowLeft className="h-5 w-5 mr-2" />
            Ir a la tienda
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto">
      {/* Header */}
      <div className="mb-8">
        <button
          onClick={() => router.push('/clientes/tienda')}
          className="flex items-center text-gray-600 hover:text-gray-900 mb-4"
        >
          <ArrowLeft className="h-5 w-5 mr-2" />
          Seguir comprando
        </button>
        <h1 className="text-3xl font-bold text-gray-900 mb-2">Carrito de Compras</h1>
        <p className="text-gray-600">{cart.length} {cart.length === 1 ? 'producto' : 'productos'} en tu carrito</p>
      </div>

      {error && (
        <div className="mb-6 bg-red-50 border border-red-200 rounded-lg p-4">
          <p className="text-red-800">{error}</p>
        </div>
      )}

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Cart items */}
        <div className="lg:col-span-2 space-y-4">
          {cart.map((item) => (
            <div
              key={item.productoId}
              className="bg-white rounded-lg shadow-md p-6 flex items-center gap-4"
            >
              {/* Product placeholder image */}
              <div className="w-20 h-20 bg-gradient-to-br from-blue-100 to-blue-200 rounded-lg flex items-center justify-center flex-shrink-0">
                <ShoppingCart className="h-8 w-8 text-blue-400" />
              </div>

              {/* Product info */}
              <div className="flex-1 min-w-0">
                <h3 className="text-lg font-semibold text-gray-900 mb-1 truncate">
                  {item.productoNombre}
                </h3>
                <p className="text-gray-600">
                  ${item.precioUnitario.toLocaleString('es-CO')} c/u
                </p>
              </div>

              {/* Quantity controls */}
              <div className="flex items-center space-x-2">
                <button
                  onClick={() => updateQuantity(item.productoId, -1)}
                  className="p-1 rounded-lg border border-gray-300 hover:bg-gray-50"
                  disabled={item.cantidad === 1}
                >
                  <Minus className="h-4 w-4" />
                </button>
                <span className="w-12 text-center font-medium">{item.cantidad}</span>
                <button
                  onClick={() => updateQuantity(item.productoId, 1)}
                  className="p-1 rounded-lg border border-gray-300 hover:bg-gray-50"
                >
                  <Plus className="h-4 w-4" />
                </button>
              </div>

              {/* Subtotal and remove */}
              <div className="text-right">
                <p className="text-lg font-bold text-gray-900 mb-2">
                  ${(item.cantidad * item.precioUnitario).toLocaleString('es-CO')}
                </p>
                <button
                  onClick={() => removeItem(item.productoId)}
                  className="text-red-600 hover:text-red-700 flex items-center justify-end"
                >
                  <Trash2 className="h-4 w-4 mr-1" />
                  <span className="text-sm">Eliminar</span>
                </button>
              </div>
            </div>
          ))}

          {/* Clear cart button */}
          <button
            onClick={clearCart}
            className="w-full py-3 border-2 border-red-300 text-red-600 rounded-lg hover:bg-red-50 transition-colors font-medium"
          >
            Vaciar carrito
          </button>
        </div>

        {/* Order summary */}
        <div className="lg:col-span-1">
          <div className="bg-white rounded-lg shadow-md p-6 sticky top-24">
            <h2 className="text-xl font-bold text-gray-900 mb-4">Resumen del pedido</h2>

            <div className="space-y-3 mb-6">
              <div className="flex justify-between text-gray-600">
                <span>Subtotal</span>
                <span>${calculateTotal().toLocaleString('es-CO')}</span>
              </div>
              <div className="flex justify-between text-gray-600">
                <span>IVA (19%)</span>
                <span>${(calculateTotal() * 0.19).toLocaleString('es-CO')}</span>
              </div>
              <div className="border-t pt-3 flex justify-between text-xl font-bold text-gray-900">
                <span>Total</span>
                <span>${(calculateTotal() * 1.19).toLocaleString('es-CO')}</span>
              </div>
            </div>

            <button
              onClick={handleCheckout}
              disabled={loading}
              className="w-full flex items-center justify-center px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:bg-gray-400 disabled:cursor-not-allowed font-medium"
            >
              {loading ? (
                <>
                  <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white mr-2"></div>
                  Procesando...
                </>
              ) : (
                <>
                  <CreditCard className="h-5 w-5 mr-2" />
                  Finalizar compra
                </>
              )}
            </button>

            <p className="text-xs text-gray-500 text-center mt-4">
              Al finalizar la compra, aceptas nuestros términos y condiciones
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

