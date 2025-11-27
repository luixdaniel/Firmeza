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
      // Mostrar TODOS los productos de la base de datos
      setProductos(productosData);
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
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Cargando productos...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="bg-red-50 border border-red-200 rounded-lg p-4">
        <p className="text-red-800">{error}</p>
        <button
          onClick={loadData}
          className="mt-2 text-red-600 hover:text-red-800 font-medium"
        >
          Intentar nuevamente
        </button>
      </div>
    );
  }

  return (
    <div>
      {/* Header */}
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">Tienda</h1>
        <p className="text-gray-600">Descubre nuestros productos disponibles</p>
      </div>

      {/* Search and Filters */}
      <div className="mb-6 space-y-4">
        <div className="flex flex-col sm:flex-row gap-4">
          {/* Search bar */}
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-5 w-5" />
            <input
              type="text"
              placeholder="Buscar productos..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>

          {/* Filter button (mobile) */}
          <button
            onClick={() => setShowFilters(!showFilters)}
            className="sm:hidden flex items-center justify-center px-4 py-2 bg-white border border-gray-300 rounded-lg hover:bg-gray-50"
          >
            <Filter className="h-5 w-5 mr-2" />
            Filtros
          </button>
        </div>

        {/* Categories filter */}
        <div className={`${showFilters ? 'block' : 'hidden'} sm:block`}>
          <div className="flex flex-wrap gap-2">
            <button
              onClick={() => setSelectedCategoria(null)}
              className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                selectedCategoria === null
                  ? 'bg-blue-600 text-white'
                  : 'bg-white text-gray-700 border border-gray-300 hover:bg-gray-50'
              }`}
            >
              Todas
            </button>
            {categorias.map((categoria) => (
              <button
                key={categoria.id}
                onClick={() => setSelectedCategoria(categoria.id)}
                className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                  selectedCategoria === categoria.id
                    ? 'bg-blue-600 text-white'
                    : 'bg-white text-gray-700 border border-gray-300 hover:bg-gray-50'
                }`}
              >
                {categoria.nombre}
              </button>
            ))}
          </div>
        </div>
      </div>

      {/* Results count */}
      <div className="mb-4">
        <p className="text-sm text-gray-600">
          {filteredProductos.length} {filteredProductos.length === 1 ? 'producto encontrado' : 'productos encontrados'}
        </p>
      </div>

      {/* Products grid */}
      {filteredProductos.length === 0 ? (
        <div className="text-center py-12">
          <p className="text-gray-500 text-lg">No se encontraron productos</p>
          {(searchTerm || selectedCategoria) && (
            <button
              onClick={() => {
                setSearchTerm('');
                setSelectedCategoria(null);
              }}
              className="mt-4 text-blue-600 hover:text-blue-700 font-medium"
            >
              Limpiar filtros
            </button>
          )}
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {filteredProductos.map((producto) => (
            <div
              key={producto.id}
              className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow"
            >
              {/* Product image placeholder */}
              <div className="h-48 bg-gradient-to-br from-blue-100 to-blue-200 flex items-center justify-center">
                {producto.imagenUrl ? (
                  <img
                    src={producto.imagenUrl}
                    alt={producto.nombre}
                    className="w-full h-full object-cover"
                  />
                ) : (
                  <div className="text-center p-4">
                    <ShoppingCart className="h-12 w-12 text-blue-400 mx-auto mb-2" />
                    <p className="text-blue-600 text-sm font-medium">{producto.nombre}</p>
                  </div>
                )}
              </div>

              {/* Product info */}
              <div className="p-4">
                <div className="mb-2 flex items-center justify-between">
                  <span className="text-xs font-medium text-blue-600 bg-blue-50 px-2 py-1 rounded">
                    {producto.categoriaNombre}
                  </span>
                  <div className="flex items-center gap-1">
                    {!producto.activo && (
                      <span className="text-xs font-medium text-red-600 bg-red-50 px-2 py-1 rounded">
                        Inactivo
                      </span>
                    )}
                    {producto.stock === 0 && (
                      <span className="text-xs font-medium text-orange-600 bg-orange-50 px-2 py-1 rounded">
                        Agotado
                      </span>
                    )}
                  </div>
                </div>
                <h3 className="text-lg font-semibold text-gray-900 mb-2 line-clamp-1">
                  {producto.nombre}
                </h3>
                <p className="text-sm text-gray-600 mb-3 line-clamp-2">
                  {producto.descripcion}
                </p>
                <div className="mb-3 text-xs text-gray-500">
                  <p><span className="font-semibold">ID:</span> {producto.id}</p>
                  <p><span className="font-semibold">Categoría ID:</span> {producto.categoriaId}</p>
                </div>
                <div className="flex items-center justify-between">
                  <div>
                    <p className="text-2xl font-bold text-gray-900">
                      ${producto.precio.toLocaleString('es-CO')}
                    </p>
                    <p className={`text-xs font-medium ${
                      producto.stock === 0 
                        ? 'text-red-600' 
                        : producto.stock < 10 
                        ? 'text-orange-600' 
                        : 'text-green-600'
                    }`}>
                      Stock: {producto.stock} unidades
                    </p>
                  </div>
                  <button
                    onClick={() => addToCart(producto)}
                    disabled={producto.stock === 0 || !producto.activo}
                    className="flex items-center justify-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:bg-gray-300 disabled:cursor-not-allowed"
                    title={
                      !producto.activo 
                        ? 'Producto inactivo' 
                        : producto.stock === 0 
                        ? 'Producto agotado' 
                        : 'Agregar al carrito'
                    }
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

