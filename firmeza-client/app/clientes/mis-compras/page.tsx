'use client';

import { useEffect, useState } from 'react';
import { ventasService } from '@/services/api';
import { Venta } from '@/types';
import { Package, Calendar, DollarSign, ChevronDown, ChevronUp } from 'lucide-react';

export default function MisComprasPage() {
  const [ventas, setVentas] = useState<Venta[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [expandedVenta, setExpandedVenta] = useState<number | null>(null);

  useEffect(() => {
    loadVentas();
  }, []);

  const loadVentas = async () => {
    try {
      setLoading(true);
      const data = await ventasService.getMisCompras();
      setVentas(data.sort((a, b) => new Date(b.fecha).getTime() - new Date(a.fecha).getTime()));
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al cargar compras');
    } finally {
      setLoading(false);
    }
  };

  const toggleExpanded = (ventaId: number) => {
    setExpandedVenta(expandedVenta === ventaId ? null : ventaId);
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-CO', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-[400px]">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Cargando tus compras...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="bg-red-50 border border-red-200 rounded-lg p-4">
        <p className="text-red-800">{error}</p>
        <button
          onClick={loadVentas}
          className="mt-2 text-red-600 hover:text-red-800 font-medium"
        >
          Intentar nuevamente
        </button>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">Mis Compras</h1>
        <p className="text-gray-600">Historial de tus pedidos realizados</p>
      </div>

      {ventas.length === 0 ? (
        <div className="text-center py-12 bg-white rounded-lg shadow-md">
          <Package className="h-24 w-24 text-gray-300 mx-auto mb-4" />
          <h2 className="text-2xl font-bold text-gray-900 mb-2">No tienes compras aún</h2>
          <p className="text-gray-600 mb-6">Explora nuestra tienda y realiza tu primera compra</p>
          <a
            href="/clientes/tienda"
            className="inline-block px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            Ir a la tienda
          </a>
        </div>
      ) : (
        <>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
            <div className="bg-white rounded-lg shadow-md p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-600 mb-1">Total de compras</p>
                  <p className="text-2xl font-bold text-gray-900">{ventas.length}</p>
                </div>
                <Package className="h-10 w-10 text-blue-600" />
              </div>
            </div>
            <div className="bg-white rounded-lg shadow-md p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-600 mb-1">Total gastado</p>
                  <p className="text-2xl font-bold text-gray-900">
                    ${ventas.reduce((sum, v) => sum + v.total, 0).toLocaleString('es-CO')}
                  </p>
                </div>
                <DollarSign className="h-10 w-10 text-green-600" />
              </div>
            </div>
            <div className="bg-white rounded-lg shadow-md p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-600 mb-1">Última compra</p>
                  <p className="text-sm font-bold text-gray-900">
                    {ventas.length > 0 ? new Date(ventas[0].fecha).toLocaleDateString('es-CO') : 'N/A'}
                  </p>
                </div>
                <Calendar className="h-10 w-10 text-purple-600" />
              </div>
            </div>
          </div>

          <div className="space-y-4">
            {ventas.map((venta) => (
              <div key={venta.id} className="bg-white rounded-lg shadow-md overflow-hidden">
                <button
                  onClick={() => toggleExpanded(venta.id)}
                  className="w-full p-6 flex items-center justify-between hover:bg-gray-50 transition-colors"
                >
                  <div className="flex items-center space-x-4">
                    <div className="bg-blue-100 rounded-full p-3">
                      <Package className="h-6 w-6 text-blue-600" />
                    </div>
                    <div className="text-left">
                      <p className="text-sm text-gray-600">Pedido #{venta.id}</p>
                      <p className="text-lg font-bold text-gray-900">
                        ${venta.total.toLocaleString('es-CO')}
                      </p>
                      <p className="text-sm text-gray-500">{formatDate(venta.fecha)}</p>
                    </div>
                  </div>
                  <div className="flex items-center space-x-4">
                    <div className="text-right hidden sm:block">
                      <p className="text-sm text-gray-600">
                        {venta.detalles.length} {venta.detalles.length === 1 ? 'producto' : 'productos'}
                      </p>
                      <span className="inline-block mt-1 px-3 py-1 bg-green-100 text-green-800 text-xs font-semibold rounded-full">
                        Completado
                      </span>
                    </div>
                    {expandedVenta === venta.id ? (
                      <ChevronUp className="h-6 w-6 text-gray-400" />
                    ) : (
                      <ChevronDown className="h-6 w-6 text-gray-400" />
                    )}
                  </div>
                </button>

                {expandedVenta === venta.id && (
                  <div className="border-t border-gray-200 bg-gray-50 p-6">
                    {/* Información de la venta */}
                    <div className="bg-white rounded-lg p-4 mb-4">
                      <h3 className="text-lg font-semibold text-gray-900 mb-3">
                        Información de la Venta
                      </h3>
                      <div className="grid grid-cols-2 md:grid-cols-3 gap-4 text-sm">
                        <div>
                          <p className="text-gray-600 font-medium">ID de Venta</p>
                          <p className="text-gray-900">#{venta.id}</p>
                        </div>
                        <div>
                          <p className="text-gray-600 font-medium">Cliente</p>
                          <p className="text-gray-900">{venta.clienteNombre}</p>
                        </div>
                        <div>
                          <p className="text-gray-600 font-medium">Cliente ID</p>
                          <p className="text-gray-900">#{venta.clienteId}</p>
                        </div>
                        <div>
                          <p className="text-gray-600 font-medium">Fecha</p>
                          <p className="text-gray-900">{formatDate(venta.fecha)}</p>
                        </div>
                        <div>
                          <p className="text-gray-600 font-medium">Productos</p>
                          <p className="text-gray-900">{venta.detalles.length} items</p>
                        </div>
                        <div>
                          <p className="text-gray-600 font-medium">Estado</p>
                          <span className="inline-block px-3 py-1 bg-green-100 text-green-800 text-xs font-semibold rounded-full">
                            Completado
                          </span>
                        </div>
                      </div>
                    </div>

                    {/* Detalles de productos */}
                    <h3 className="text-lg font-semibold text-gray-900 mb-4">
                      Productos Comprados
                    </h3>
                    <div className="space-y-3">
                      {venta.detalles.map((detalle, index) => (
                        <div
                          key={index}
                          className="bg-white p-4 rounded-lg"
                        >
                          <div className="flex items-start justify-between mb-2">
                            <div className="flex-1">
                              <p className="font-medium text-gray-900 text-lg">{detalle.productoNombre}</p>
                              <p className="text-xs text-gray-500 mt-1">
                                <span className="font-semibold">ID Detalle:</span> {detalle.id} | <span className="font-semibold">ID Producto:</span> {detalle.productoId}
                              </p>
                            </div>
                            <div className="text-right ml-4">
                              <p className="text-sm text-gray-600">Subtotal</p>
                              <p className="font-bold text-gray-900 text-lg">
                                ${detalle.subtotal.toLocaleString('es-CO')}
                              </p>
                            </div>
                          </div>
                          <div className="flex items-center gap-6 mt-3 pt-3 border-t border-gray-100">
                            <div>
                              <p className="text-xs text-gray-500">Cantidad</p>
                              <p className="text-sm font-semibold text-gray-900">{detalle.cantidad} unidades</p>
                            </div>
                            <div>
                              <p className="text-xs text-gray-500">Precio unitario</p>
                              <p className="text-sm font-semibold text-gray-900">${detalle.precioUnitario.toLocaleString('es-CO')}</p>
                            </div>
                            <div>
                              <p className="text-xs text-gray-500">Total</p>
                              <p className="text-sm font-semibold text-blue-600">
                                ${(detalle.cantidad * detalle.precioUnitario).toLocaleString('es-CO')}
                              </p>
                            </div>
                          </div>
                        </div>
                      ))}
                    </div>

                    {/* Resumen del total */}
                    <div className="mt-6 bg-white rounded-lg p-4">
                      <h4 className="font-semibold text-gray-900 mb-3">Resumen del Pedido</h4>
                      <div className="space-y-2">
                        <div className="flex justify-between text-gray-600">
                          <span>Subtotal ({venta.detalles.length} productos)</span>
                          <span>${venta.total.toLocaleString('es-CO')}</span>
                        </div>
                        <div className="flex justify-between text-gray-600 text-sm">
                          <span>IVA incluido</span>
                          <span>19%</span>
                        </div>
                        <div className="pt-3 mt-3 border-t border-gray-200 flex justify-between items-center">
                          <span className="text-xl font-bold text-gray-900">Total Pagado</span>
                          <span className="text-2xl font-bold text-blue-600">
                            ${venta.total.toLocaleString('es-CO')}
                          </span>
                        </div>
                      </div>
                    </div>
                  </div>
                )}
              </div>
            ))}
          </div>
        </>
      )}
    </div>
  );
}

