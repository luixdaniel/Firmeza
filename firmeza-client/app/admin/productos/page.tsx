"use client";

import { useEffect, useState } from "react";
import { productosService } from "@/services/api";
import { Producto } from "@/types";
import { Plus, Search, Edit2, Trash2, Eye, Package } from "lucide-react";

export default function AdminProductosPage() {
  const [productos, setProductos] = useState<Producto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    // El layout ya verifica la autenticación
    loadProductos();
  }, []);

  const loadProductos = async () => {
    try {
      setLoading(true);
      const data = await productosService.getAll();
      setProductos(data);
    } catch (err: any) {
      setError(err.response?.data?.message || "Error al cargar productos");
    } finally {
      setLoading(false);
    }
  };

  const filteredProductos = productos.filter(
    (p) =>
      p.nombre.toLowerCase().includes(searchTerm.toLowerCase()) ||
      p.descripcion?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const valorInventario = productos.reduce(
    (sum, p) => sum + p.precio * p.stock,
    0
  );

  if (loading) {
    return (
      <div className="flex items-center justify-center py-12">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div>
      <div className="mb-6 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">
            Gestión de Productos
          </h1>
          <p className="text-gray-600 mt-1">Administra el catálogo de productos</p>
        </div>
        <button className="flex items-center space-x-2 bg-primary-600 hover:bg-primary-700 text-white px-6 py-3 rounded-lg font-semibold transition shadow-lg">
          <Plus className="h-5 w-5" />
          <span>Nuevo Producto</span>
        </button>
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
              <div className="text-sm text-gray-600 mb-1">Total Productos</div>
              <div className="text-3xl font-bold text-gray-900">
                {productos.length}
              </div>
            </div>
            <Package className="h-10 w-10 text-gray-300" />
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="flex items-center justify-between">
            <div>
              <div className="text-sm text-gray-600 mb-1">En Stock</div>
              <div className="text-3xl font-bold text-green-600">
                {productos.filter((p) => p.stock > 0).length}
              </div>
            </div>
            <Package className="h-10 w-10 text-green-300" />
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="flex items-center justify-between">
            <div>
              <div className="text-sm text-gray-600 mb-1">Sin Stock</div>
              <div className="text-3xl font-bold text-red-600">
                {productos.filter((p) => p.stock === 0).length}
              </div>
            </div>
            <Package className="h-10 w-10 text-red-300" />
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow">
          <div className="flex items-center justify-between">
            <div>
              <div className="text-sm text-gray-600 mb-1">Valor Inventario</div>
              <div className="text-2xl font-bold text-primary-600">
                ${valorInventario.toFixed(2)}
              </div>
            </div>
            <Package className="h-10 w-10 text-primary-300" />
          </div>
        </div>
      </div>

      {/* Barra de búsqueda */}
      <div className="mb-6">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
          <input
            type="text"
            placeholder="Buscar productos..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
          />
        </div>
      </div>

      {/* Grid de Productos */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {filteredProductos.map((producto) => (
          <div
            key={producto.id}
            className="bg-white rounded-lg shadow hover:shadow-lg transition-shadow overflow-hidden"
          >
            <div className="h-48 bg-gradient-to-br from-primary-100 to-primary-200 flex items-center justify-center">
              <div className="text-6xl">📦</div>
            </div>

            <div className="p-4">
              <h3 className="font-semibold text-lg text-gray-900 mb-1">
                {producto.nombre}
              </h3>
              <p className="text-sm text-gray-600 mb-3 line-clamp-2">
                {producto.descripcion || "Sin descripción"}
              </p>

              <div className="flex items-center justify-between mb-3">
                <div className="text-2xl font-bold text-primary-600">
                  ${producto.precio.toFixed(2)}
                </div>
                <div
                  className={`text-sm font-medium px-3 py-1 rounded-full ${
                    producto.stock > 10
                      ? "bg-green-100 text-green-800"
                      : producto.stock > 0
                      ? "bg-yellow-100 text-yellow-800"
                      : "bg-red-100 text-red-800"
                  }`}
                >
                  Stock: {producto.stock}
                </div>
              </div>

              <div className="flex space-x-2">
                <button
                  title="Ver detalles"
                  className="flex-1 p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition"
                >
                  <Eye className="h-4 w-4 mx-auto" />
                </button>
                <button
                  title="Editar"
                  className="flex-1 p-2 text-green-600 hover:bg-green-50 rounded-lg transition"
                >
                  <Edit2 className="h-4 w-4 mx-auto" />
                </button>
                <button
                  title="Eliminar"
                  className="flex-1 p-2 text-red-600 hover:bg-red-50 rounded-lg transition"
                >
                  <Trash2 className="h-4 w-4 mx-auto" />
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>

      {filteredProductos.length === 0 && (
        <div className="text-center py-12 bg-white rounded-lg shadow">
          <p className="text-gray-500 text-lg">
            {searchTerm
              ? "No se encontraron productos con ese criterio"
              : "No hay productos registrados"}
          </p>
        </div>
      )}
    </div>
  );
}
