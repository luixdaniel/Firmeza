'use client';

import { useEffect, useState } from 'react';
import { ventasService } from '@/services/api';
import { Venta } from '@/types';
import { Search, FileText, Download, Eye, Calendar, DollarSign } from 'lucide-react';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';

export default function AdminVentasPage() {
  const [ventas, setVentas] = useState<Venta[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    // El layout ya verifica la autenticación
    loadVentas();
  }, []);

  const loadVentas = async () => {
    try {
      setLoading(true);
      const data = await ventasService.getAll();
      setVentas(data);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al cargar ventas');
    } finally {
      setLoading(false);
    }
  };

  const filteredVentas = ventas.filter((v) => {
    const searchLower = searchTerm.toLowerCase();
    return (
      v.id.toString().includes(searchLower) ||
      v.cliente?.nombreCompleto?.toLowerCase().includes(searchLower)
    );
  });

  const totalVentas = ventas.reduce((sum, v) => sum + v.total, 0);
  const promedioVenta = ventas.length > 0 ? totalVentas / ventas.length : 0;

  if (loading) {
    return (
      <div className="flex items-center justify-center py-12">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div>
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Gestión de Ventas</h1>
        <p className="text-gray-600 mt-1">Historial completo de transacciones</p>
      </div>

      {error && (
        <div className="mb-4 p-4 bg-red-50 border border-red-200 text-red-700 rounded-lg">
          {error}
        </div>
      )}

      {/* Stats */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
        <div className="bg-white p-6 rounded-lg shadow">
          <div className="flex items-center justify-between">
            <div>
              <div className="text-sm text-gray-600 mb-1">Total Ventas</div>
              <div className="text-3xl font-bold text-gray-900">{ventas.length}</div>
            </div>
            <FileText className="h-10 w-10 text-gray-300" />
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="flex items-center justify-between">
            <div>
              <div className="text-sm text-gray-600 mb-1">Ingresos Totales</div>
              <div className="text-2xl font-bold text-green-600">
                ${totalVentas.toFixed(2)}
              </div>
            </div>
            <DollarSign className="h-10 w-10 text-green-300" />
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="flex items-center justify-between">
            <div>
              <div className="text-sm text-gray-600 mb-1">Promedio por Venta</div>
              <div className="text-2xl font-bold text-primary-600">
                ${promedioVenta.toFixed(2)}
              </div>
            </div>
            <DollarSign className="h-10 w-10 text-primary-300" />
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="flex items-center justify-between">
            <div>
              <div className="text-sm text-gray-600 mb-1">Este Mes</div>
              <div className="text-3xl font-bold text-orange-600">
                {ventas.filter((v) => {
                  const ventaDate = new Date(v.fecha);
                  const now = new Date();
                  return (
                    ventaDate.getMonth() === now.getMonth() &&
                    ventaDate.getFullYear() === now.getFullYear()
                  );
                }).length}
              </div>
            </div>
            <Calendar className="h-10 w-10 text-orange-300" />
          </div>
        </div>
      </div>

      {/* Barra de búsqueda */}
      <div className="mb-6">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
          <input
            type="text"
            placeholder="Buscar por ID de venta o nombre de cliente..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
          />
        </div>
      </div>

      {/* Lista de ventas */}
      <div className="space-y-4">
        {filteredVentas.map((venta) => (
          <div
            key={venta.id}
            className="bg-white rounded-lg shadow hover:shadow-lg transition-shadow overflow-hidden"
          >
            <div className="p-6">
              <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
                {/* Información principal */}
                <div className="flex-1">
                  <div className="flex items-center space-x-3 mb-2">
                    <div className="bg-primary-100 p-2 rounded">
                      <FileText className="h-5 w-5 text-primary-600" />
                    </div>
                    <div>
                      <h3 className="font-semibold text-gray-900">
                        Venta #{venta.id}
                      </h3>
                      <p className="text-sm text-gray-600">
                        {format(new Date(venta.fecha), "dd 'de' MMMM 'de' yyyy, HH:mm", {
                          locale: es,
                        })}
                      </p>
                    </div>
                  </div>

                  {/* Cliente */}
                  <div className="ml-11 mt-3">
                    <div className="text-sm text-gray-600 mb-2">
                      <span className="font-medium">Cliente:</span>{' '}
                      {venta.cliente?.nombreCompleto || 'N/A'}
                      {venta.cliente?.email && (
                        <span className="text-gray-500"> ({venta.cliente.email})</span>
                      )}
                    </div>

                    {/* Productos */}
                    <div className="text-sm text-gray-600">
                      <span className="font-medium">Productos:</span>{' '}
                      {venta.detalleVentas?.length || 0} items
                      {venta.detalleVentas && venta.detalleVentas.length > 0 && (
                        <div className="mt-2 space-y-1">
                          {venta.detalleVentas.map((detalle, idx) => (
                            <div key={idx} className="text-gray-500 ml-4">
                              • {detalle.producto?.nombre || 'Producto'} x{' '}
                              {detalle.cantidad} - ${detalle.precioUnitario.toFixed(2)} c/u
                            </div>
                          ))}
                        </div>
                      )}
                    </div>
                  </div>
                </div>

                {/* Total y acciones */}
                <div className="flex flex-col items-end space-y-3">
                  <div className="text-right">
                    <div className="text-sm text-gray-600">Total</div>
                    <div className="text-3xl font-bold text-primary-600">
                      ${venta.total.toFixed(2)}
                    </div>
                  </div>

                  <div className="flex space-x-2">
                    <button
                      title="Ver detalle"
                      className="flex items-center space-x-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition text-sm"
                    >
                      <Eye className="h-4 w-4" />
                      <span>Ver</span>
                    </button>
                    <button
                      title="Descargar PDF"
                      className="flex items-center space-x-2 px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition text-sm"
                    >
                      <Download className="h-4 w-4" />
                      <span>PDF</span>
                    </button>
                  </div>
                </div>
              </div>
            </div>

            {/* Badge de estado */}
            <div className="bg-green-50 px-6 py-3 border-t border-green-100">
              <div className="flex items-center justify-between">
                <span className="text-sm font-medium text-green-700">
                  ✓ Venta completada
                </span>
                <span className="text-xs text-green-600">
                  {format(new Date(venta.fecha), 'HH:mm', { locale: es })}
                </span>
              </div>
            </div>
          </div>
        ))}
      </div>

      {filteredVentas.length === 0 && (
        <div className="text-center py-12 bg-white rounded-lg shadow">
          <p className="text-gray-500 text-lg">
            {searchTerm
              ? 'No se encontraron ventas con ese criterio'
              : 'No hay ventas registradas'}
          </p>
        </div>
      )}
    </div>
  );
}
