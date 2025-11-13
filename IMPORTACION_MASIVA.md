# Sistema de Importación Masiva - Firmeza

## 🎯 Funcionalidad Implementada

Se ha implementado un sistema completo de **carga masiva de datos desnormalizados** mediante archivos Excel (.xlsx) usando **EPPlus**.

## 📋 Características Principales

### 1. **Lectura Inteligente de Excel**
- ✅ Detecta automáticamente las columnas del archivo
- ✅ Soporta múltiples formatos de encabezados (español/inglés)
- ✅ Ignora filas vacías o incompletas
- ✅ Maneja datos mezclados de diferentes entidades

### 2. **Normalización Automática**
El sistema identifica y separa automáticamente:
- **Productos**: Código, Nombre, Descripción, Precio, Stock, Categoría
- **Clientes**: Nombre, Apellido, Email, Teléfono, Dirección, Documento
- **Ventas**: Cantidad, Precio, Fecha, Método de Pago, Estado

### 3. **Validación de Datos**
- ✅ Valida campos obligatorios
- ✅ Verifica tipos de datos (números, fechas, textos)
- ✅ Previene duplicados por email/código
- ✅ Valida stock disponible para ventas

### 4. **Procesamiento Inteligente**
- ✅ **Crea** registros nuevos si no existen
- ✅ **Actualiza** registros existentes si ya están en la BD
- ✅ Relaciona automáticamente productos con categorías
- ✅ Relaciona ventas con clientes y productos
- ✅ Actualiza inventario automáticamente

### 5. **Log de Errores Detallado**
- 📊 Muestra número de fila con error
- 📊 Indica el campo problemático
- 📊 Describe el tipo de error
- 📊 Identifica la entidad afectada

### 6. **Reportes Completos**
Después de la importación muestra:
- Total de filas procesadas
- Filas exitosas vs con errores
- Productos creados/actualizados
- Clientes creados/actualizados
- Ventas creadas
- Log detallado de errores

## 🗂️ Archivos Creados

### Modelos
```
Models/ImportacionMasiva/
├── ImportResultado.cs          # Resultado de la importación con estadísticas
└── DatosDesnormalizados.cs     # Modelo para datos mezclados del Excel
```

### Servicios
```
Interfaces/Services/
└── IImportacionMasivaService.cs

Services/
└── ImportacionMasivaService.cs  # Lógica completa de importación
```

### Controlador y Vistas
```
Areas/Admin/Controllers/
└── ImportacionController.cs

Areas/Admin/Views/Importacion/
├── Index.cshtml                 # Formulario de carga
└── Resultado.cshtml             # Reporte de importación
```

## 📊 Formatos de Columnas Soportados

El sistema reconoce automáticamente estas variaciones de nombres:

### Productos
- `Codigo`, `CodigoProducto`, `SKU`
- `Producto`, `NombreProducto`, `Nombre`
- `Descripcion`, `DescripcionProducto`
- `Precio`, `PrecioProducto`, `PrecioUnitario`
- `Stock`, `Cantidad`, `Existencia`
- `Categoria`, `CategoriaProducto`

### Clientes
- `CodigoCliente`, `IdCliente`
- `Cliente`, `NombreCliente`
- `Apellido`, `ApellidoCliente`
- `Email`, `Correo`, `EmailCliente`
- `Telefono`, `TelefonoCliente`, `Celular`
- `Direccion`, `DireccionCliente`
- `Documento`, `DNI`, `Cedula`, `RUT`

### Ventas
- `Factura`, `NumeroFactura`, `NroFactura`
- `Fecha`, `FechaVenta`
- `CantidadVendida`, `CantidadVenta`, `Unidades`
- `PrecioVenta`, `PrecioUnidad`
- `MetodoPago`, `Pago`, `FormaPago`
- `Estado`, `EstadoVenta`

## 🚀 Uso

1. **Acceder al módulo**
   - Iniciar sesión como Administrador
   - Ir al Panel Admin
   - Click en "Importar desde Excel"

2. **Descargar plantilla** (opcional)
   - Plantilla Completa (todos los campos)
   - Solo Productos
   - Solo Clientes
   - Solo Ventas

3. **Preparar archivo Excel**
   - Usar los nombres de columna indicados
   - Primera fila = encabezados
   - Siguientes filas = datos
   - Puede mezclar datos de diferentes entidades

4. **Importar archivo**
   - Seleccionar archivo .xlsx
   - Elegir tipo de importación (Auto recomendado)
   - Click en "Importar Datos"

5. **Revisar resultados**
   - Ver estadísticas de importación
   - Revisar errores si los hay
   - Verificar datos importados

## 📝 Ejemplos de Archivos

### Ejemplo 1: Solo Productos
```
| Codigo  | NombreProducto | Precio  | Stock | Categoria   |
|---------|----------------|---------|-------|-------------|
| PROD001 | Laptop Dell    | 899.99  | 10    | Tecnología  |
| PROD002 | Mouse Logitech | 25.50   | 50    | Accesorios  |
```

### Ejemplo 2: Solo Clientes
```
| NombreCliente | Apellido | Email                  | Telefono  |
|---------------|----------|------------------------|-----------|
| Juan          | Pérez    | juan.perez@example.com | 555-1234  |
| María         | García   | maria.g@example.com    | 555-5678  |
```

### Ejemplo 3: Datos Mezclados (Completo)
```
| Codigo  | NombreProducto | Precio | Stock | NombreCliente | Email                | CantidadVendida | MetodoPago |
|---------|----------------|--------|-------|---------------|----------------------|-----------------|------------|
| PROD001 | Laptop Dell    | 899.99 | 10    | Juan Pérez    | juan@example.com     | 2               | Tarjeta    |
| PROD002 | Mouse Logitech | 25.50  | 50    | María García  | maria@example.com    | 5               | Efectivo   |
```

## 🔧 Configuración

El servicio ya está registrado en `Program.cs`:
```csharp
builder.Services.AddScoped<IImportacionMasivaService, ImportacionMasivaService>();
```

EPPlus está configurado con licencia **NonCommercial** (cambiar a Commercial si es necesario).

## ⚠️ Notas Importantes

1. **Categorías**: Se crean automáticamente si no existen
2. **Email único**: Los clientes se identifican por email
3. **Código único**: Los productos se identifican por código
4. **Stock**: Las ventas descuentan automáticamente del inventario
5. **Zona horaria**: Las fechas se guardan en UTC
6. **Validaciones**: Solo se importan datos válidos
7. **Transacciones**: Cada fila se procesa independientemente

## 🎨 Interfaz

- Diseño moderno con Bootstrap 5
- Iconos de Bootstrap Icons
- Alertas informativas
- Tablas responsivas
- Cards con estadísticas
- Acordeones para ejemplos

## 📈 Próximas Mejoras Sugeridas

- [ ] Exportar log de errores a Excel
- [ ] Importación en segundo plano para archivos grandes
- [ ] Barra de progreso en tiempo real
- [ ] Vista previa antes de importar
- [ ] Rollback en caso de errores críticos
- [ ] Soporte para más formatos (.csv, .xls)

