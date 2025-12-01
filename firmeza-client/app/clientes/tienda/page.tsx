'use client';

import { useEffect, useState } from 'react';
import { productosService, categoriasService } from '@/services/api';
import { Producto, Categoria } from '@/types';
import { ShoppingCart, Search, Filter } from 'lucide-react';

export default function TiendaPage() {
  const [productos, setProductos] = useState<Producto[]>([]);
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategoria, setSelectedCategoria] = useState<number | null>(null);
  const [showFilters, setShowFilters] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [productosData, categoriasData] = await Promise.all([
        productosService.getAll(),
        categoriasService.getAll(),
      ]);
      setProductos(productosData.filter(p => p.activo && p.stock > 0));
      setCategorias(categoriasData);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al cargar productos');
    } finally {
      setLoading(false);
    }
  };

  const addToCart = (producto: Producto) => {
    const cart = JSON.parse(localStorage.getItem('cart') || '[]');
    const existingItem = cart.find((item: any) => item.productoId === producto.id);

    if (existingItem) {
      existingItem.cantidad += 1;
    } else {
      cart.push({
        productoId: producto.id,
        productoNombre: producto.nombre,
        cantidad: 1,
        precioUnitario: producto.precio,
      });
    }

    localStorage.setItem('cart', JSON.stringify(cart));
    window.dispatchEvent(new Event('cartUpdated'));

    // Mostrar notificación
    alert(`${producto.nombre} añadido al carrito`);
  };

  const filteredProductos = productos.filter((producto) => {
    const matchesSearch = producto.nombre.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         producto.descripcion.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesCategoria = !selectedCategoria || producto.categoriaId === selectedCategoria;
    return matchesSearch && matchesCategoria;
  });

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-[400px]">
        <div className="text-center bg-gradient-to-br from-slate-100 to-orange-50 p-12 rounded-2xl border-4 border-orange-200 shadow-xl">
          <div className="text-6xl mb-6 animate-bounce">🏗️</div>
          <div className="animate-spin rounded-full h-16 w-16 border-b-4 border-orange-600 mx-auto mb-4"></div>
          <p className="text-xl font-bold text-slate-700">Cargando catálogo...</p>
          <p className="text-sm text-slate-500 mt-2">Preparando productos industriales</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="bg-gradient-to-r from-red-50 to-orange-50 border-l-4 border-red-600 rounded-r-xl p-6 shadow-lg">
        <div className="flex items-start gap-4">
          <div className="text-4xl">⚠️</div>
          <div className="flex-1">
            <h3 className="text-xl font-bold text-red-800 mb-2">Error al Cargar</h3>
            <p className="text-red-700 font-medium mb-4">{error}</p>
            <button
              onClick={loadData}
              className="bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white font-bold px-6 py-3 rounded-lg shadow-md transition-all"
            >
              🔄 Intentar Nuevamente
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div>
      {/* Header Industrial */}
      <div className="mb-8 bg-gradient-to-r from-slate-800 to-slate-700 rounded-xl p-6 border-b-4 border-orange-600 shadow-xl">
        <div className="flex items-center gap-4">
          <div className="text-5xl">🏪</div>
          <div>
            <h1 className="text-4xl font-bold text-white mb-2">Catálogo de Productos</h1>
            <p className="text-gray-300 font-medium">Insumos de construcción y equipos industriales</p>
          </div>
        </div>
      </div>

      {/* Search and Filters Industrial */}
      <div className="mb-6 space-y-4">
        <div className="flex flex-col sm:flex-row gap-4">
          {/* Search bar Industrial */}
          <div className="flex-1 relative">
            <Search className="absolute left-4 top-1/2 transform -translate-y-1/2 text-orange-500 h-6 w-6" />
            <input
              type="text"
              placeholder="🔍 Buscar materiales, herramientas..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-12 pr-4 py-4 border-2 border-slate-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 font-medium text-gray-900 placeholder-slate-400 shadow-sm"
            />
          </div>

          {/* Filter button (mobile) */}
          <button
            onClick={() => setShowFilters(!showFilters)}
            className="sm:hidden flex items-center justify-center px-6 py-4 bg-gradient-to-r from-orange-600 to-orange-700 text-white font-bold rounded-lg hover:from-orange-700 hover:to-orange-800 shadow-lg"
          >
            <Filter className="h-5 w-5 mr-2" />
            Filtros
          </button>
        </div>

        {/* Categories filter Industrial */}
        <div className={`${showFilters ? 'block' : 'hidden'} sm:block bg-white p-4 rounded-xl border-2 border-slate-200 shadow-md`}>
          <div className="flex items-center gap-2 mb-3">
            <span className="text-xl">📂</span>
            <h3 className="font-bold text-slate-700">Categorías:</h3>
          </div>
          <div className="flex flex-wrap gap-3">
            <button
              onClick={() => setSelectedCategoria(null)}
              className={`px-5 py-2.5 rounded-lg font-bold transition-all shadow-sm ${
                selectedCategoria === null
                  ? 'bg-gradient-to-r from-orange-600 to-orange-700 text-white shadow-lg scale-105'
                  : 'bg-slate-100 text-slate-700 border-2 border-slate-300 hover:bg-slate-200 hover:border-orange-500'
              }`}
            >
              🏗️ Todos
            </button>
            {categorias.map((categoria) => (
              <button
                key={categoria.id}
                onClick={() => setSelectedCategoria(categoria.id)}
                className={`px-5 py-2.5 rounded-lg font-bold transition-all shadow-sm ${
                  selectedCategoria === categoria.id
                    ? 'bg-gradient-to-r from-orange-600 to-orange-700 text-white shadow-lg scale-105'
                    : 'bg-slate-100 text-slate-700 border-2 border-slate-300 hover:bg-slate-200 hover:border-orange-500'
                }`}
              >
                {categoria.nombre}
              </button>
            ))}
          </div>
        </div>
      </div>

      {/* Results count Industrial */}
      <div className="mb-6 bg-gradient-to-r from-orange-50 to-yellow-50 border-l-4 border-orange-600 rounded-r-lg p-4 shadow-sm">
        <p className="text-sm font-bold text-slate-700 flex items-center gap-2">
          <span className="text-xl">📊</span>
          {filteredProductos.length} {filteredProductos.length === 1 ? 'producto disponible' : 'productos disponibles'}
        </p>
      </div>

      {/* Products grid Industrial */}
      {filteredProductos.length === 0 ? (
        <div className="text-center py-16 bg-gradient-to-br from-slate-50 to-gray-100 rounded-xl border-2 border-dashed border-slate-300">
          <div className="text-7xl mb-4">📦</div>
          <p className="text-slate-600 text-xl font-bold mb-2">No se encontraron productos</p>
          <p className="text-slate-500 mb-4">Intenta con otros criterios de búsqueda</p>
          {(searchTerm || selectedCategoria) && (
            <button
              onClick={() => {
                setSearchTerm('');
                setSelectedCategoria(null);
              }}
              className="mt-4 bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white font-bold px-6 py-3 rounded-lg shadow-lg transition-all"
            >
              🔄 Limpiar Filtros
            </button>
          )}
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {filteredProductos.map((producto) => (
            <div
              key={producto.id}
              className="bg-white rounded-xl shadow-lg overflow-hidden hover:shadow-2xl transition-all hover:scale-105 border-2 border-slate-200"
            >
              {/* Product image Industrial */}
              <div className="h-48 bg-gradient-to-br from-slate-200 via-gray-200 to-orange-100 flex items-center justify-center relative">
                {producto.imagenUrl ? (
                  <img
                    src={producto.imagenUrl}
                    alt={producto.nombre}
                    className="w-full h-full object-cover"
                  />
                ) : (
                  <div className="text-center p-4">
                    <div className="text-6xl mb-2">🧱</div>
                    <p className="text-slate-600 text-sm font-bold">{producto.nombre}</p>
                  </div>
                )}
                {producto.stock < 10 && (
                  <div className="absolute top-2 right-2 bg-red-600 text-white text-xs font-bold px-3 py-1 rounded-full shadow-lg">
                    ⚠️ Últimas unidades
                  </div>
                )}
              </div>

              {/* Product info Industrial */}
              <div className="p-5">
                <div className="mb-3">
                  <span className="text-xs font-bold text-orange-700 bg-orange-100 px-3 py-1.5 rounded-full border border-orange-300">
                    {producto.categoriaNombre}
                  </span>
                </div>
                <h3 className="text-xl font-bold text-slate-800 mb-2 line-clamp-1">
                  {producto.nombre}
                </h3>
                <p className="text-sm text-slate-600 mb-4 line-clamp-2 font-medium">
                  {producto.descripcion}
                </p>
                <div className="flex items-center justify-between pt-4 border-t-2 border-slate-200">
                  <div>
                    <p className="text-3xl font-bold text-orange-600">
                      ${producto.precio.toLocaleString('es-CO')}
                    </p>
                    <p className="text-xs text-slate-500 font-semibold mt-1 flex items-center gap-1">
                      <span className="text-base">📦</span>
                      Stock: {producto.stock} uds
                    </p>
                  </div>
                  <button
                    onClick={() => addToCart(producto)}
                    disabled={producto.stock === 0}
                    className="flex items-center justify-center px-5 py-3 bg-gradient-to-r from-orange-600 to-orange-700 hover:from-orange-700 hover:to-orange-800 text-white rounded-lg transition-all shadow-md hover:shadow-lg disabled:from-gray-300 disabled:to-gray-400 disabled:cursor-not-allowed font-bold"
                    title="Agregar al carrito"
                  >
                    <ShoppingCart className="h-5 w-5" />
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

