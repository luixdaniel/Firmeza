# Exportación de Datos y Generación de Recibos PDF

## 📋 Funcionalidades Implementadas

### 1. Exportación de Datos

El sistema ahora permite exportar información a **Excel** y **PDF** desde las siguientes secciones:

#### Productos
- **Exportar a Excel**: Genera un archivo `.xlsx` con todos los productos, incluyendo:
  - ID, Nombre, Descripción, Precio, Stock, Categoría
  - Formato de moneda para precios
  - Columnas autoajustables

- **Exportar a PDF**: Genera un documento PDF con listado de productos en formato tabla

#### Clientes
- **Exportar a Excel**: Genera un archivo `.xlsx` con todos los clientes, incluyendo:
  - ID, Nombre, Apellido, Email, Teléfono, Dirección, Documento, Fecha de Registro, Estado
  - Formato de fecha
  - Columnas autoajustables

- **Exportar a PDF**: Genera un documento PDF con listado de clientes en formato horizontal

#### Ventas
- **Exportar a Excel**: Genera un archivo `.xlsx` con todas las ventas, incluyendo:
  - ID, Número de Factura, Fecha, Cliente, Subtotal, IVA, Total, Método de Pago, Estado, Vendedor
  - Formato de moneda y fecha
  - **Totales automáticos** al final (suma de Subtotal, IVA y Total)
  - Filtrado opcional por rango de fechas

- **Exportar a PDF**: Genera un documento PDF con reporte de ventas en formato horizontal
  - Incluye totales generales
  - Filtrado opcional por rango de fechas

### 2. Generación Automática de Recibos PDF

Al registrar una nueva venta, el sistema **genera automáticamente** un recibo en PDF con:

#### Contenido del Recibo
- **Encabezado**: Logo de la empresa y número de factura
- **Datos del Cliente**:
  - Nombre completo
  - Email
  - Teléfono
  - Dirección
  - Número de documento

- **Datos de la Venta**:
  - Fecha y hora de la venta
  - Método de pago
  - Estado
  - Vendedor (si aplica)

- **Detalle de Productos**:
  - Tabla con: Cantidad, Producto, Precio Unitario, Subtotal
  - Formato de moneda

- **Totales**:
  - Subtotal
  - IVA (16%)
  - **Total destacado** en verde

- **Pie de página**: Fecha de generación y número de página

#### Almacenamiento
- Los recibos se guardan automáticamente en: `wwwroot/recibos/`
- Formato de nombre: `Recibo_{NumeroFactura}_{VentaId}.pdf`

#### Descarga desde la Interfaz
En la vista de **Ventas**, cada registro tiene un botón **"Recibo"** que permite:
- Descargar el recibo en PDF
- Ver el recibo en el navegador

## 🛠️ Tecnologías Utilizadas

- **QuestPDF**: Generación moderna de PDFs con diseño fluido
- **EPPlus**: Generación de archivos Excel (.xlsx)
- **Bootstrap Icons**: Iconos para los botones de exportación

## 📂 Estructura de Archivos Creados

```
Firmeza.Web/
├── Interfaces/
│   └── Services/
│       ├── IExportacionService.cs
│       └── IPdfService.cs
├── Services/
│   ├── ExportacionService.cs
│   └── PdfService.cs
├── Areas/
│   └── Admin/
│       ├── Controllers/
│       │   └── ExportacionController.cs
│       └── Views/
│           ├── Productos/Index.cshtml (actualizado)
│           ├── Clientes/Index.cshtml (actualizado)
│           └── Ventas/Index.cshtml (actualizado)
└── wwwroot/
    └── recibos/ (carpeta generada automáticamente)
```

## 🚀 Uso

### Exportar Datos

1. Navega a la sección deseada (Productos, Clientes o Ventas)
2. Haz clic en el botón **"Exportar Excel"** o **"Exportar PDF"**
3. El archivo se descargará automáticamente con el nombre: `{Tipo}_{FechaHora}.xlsx` o `.pdf`

### Descargar Recibo de Venta

1. Ve a la sección de **Ventas**
2. En la fila de la venta deseada, haz clic en el botón **"Recibo"** (ícono PDF verde)
3. El recibo se descargará automáticamente

### Filtrar Ventas por Fecha (Exportación)

Para exportar ventas de un período específico, usa los parámetros de URL:
```
/Admin/Exportacion/ExportarVentasExcel?fechaInicio=2025-01-01&fechaFin=2025-12-31
/Admin/Exportacion/ExportarVentasPdf?fechaInicio=2025-01-01&fechaFin=2025-12-31
```

## 🔧 Configuración

Los servicios ya están registrados en `Program.cs`:
```csharp
builder.Services.AddScoped<IExportacionService, ExportacionService>();
builder.Services.AddScoped<IPdfService, PdfService>();
```

## 📝 Notas Importantes

- Los recibos PDF se generan automáticamente al crear una venta
- Si la generación del PDF falla, la venta se registra de todos modos (no se interrumpe el proceso)
- La carpeta `wwwroot/recibos/` se crea automáticamente si no existe
- Los archivos Excel usan licencia **NonCommercial** de EPPlus
- Los PDFs usan licencia **Community** de QuestPDF

## 🎨 Personalización

Para personalizar los diseños de los PDFs, edita los métodos en:
- `Services/ExportacionService.cs` (para listados)
- `Services/PdfService.cs` (para recibos de ventas)

Los colores, fuentes y estilos pueden modificarse usando la API fluida de QuestPDF.

## 🐛 Solución de Problemas

### El recibo no se genera
- Verifica que la carpeta `wwwroot/recibos/` tenga permisos de escritura
- Revisa los logs de la aplicación para ver errores específicos

### Error al exportar a Excel
- Asegúrate de que el paquete EPPlus esté instalado correctamente
- Verifica la licencia en el código: `ExcelPackage.LicenseContext = LicenseContext.NonCommercial;`

### Botones de exportación no aparecen
- Verifica que las vistas se hayan actualizado correctamente
- Limpia la caché del navegador (Ctrl+F5)

