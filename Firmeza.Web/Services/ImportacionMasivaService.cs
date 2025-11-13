using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;
using Firmeza.Web.Models.ImportacionMasiva;
using OfficeOpenXml;
using System.Globalization;

namespace Firmeza.Web.Services;

public class ImportacionMasivaService : IImportacionMasivaService
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IVentaRepository _ventaRepository;

    public ImportacionMasivaService(
        IProductoRepository productoRepository,
        ICategoriaRepository categoriaRepository,
        IClienteRepository clienteRepository,
        IVentaRepository ventaRepository)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
        _clienteRepository = clienteRepository;
        _ventaRepository = ventaRepository;
        
        // Configurar licencia de EPPlus (NonCommercial o Commercial)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<ImportResultado> ImportarDesdeExcelAsync(Stream archivoExcel, string tipoImportacion = "Auto")
    {
        var resultado = new ImportResultado();
        
        try
        {
            // Leer datos del Excel
            var datosDesnormalizados = await LeerDatosExcelAsync(archivoExcel);
            resultado.TotalFilas = datosDesnormalizados.Count;
            
            if (datosDesnormalizados.Count == 0)
            {
                resultado.Exitoso = false;
                resultado.Mensaje = "El archivo no contiene datos válidos.";
                return resultado;
            }
            
            // Normalizar y guardar datos
            resultado = await NormalizarYGuardarDatosAsync(datosDesnormalizados);
            resultado.TotalFilas = datosDesnormalizados.Count;
            resultado.Exitoso = resultado.FilasConError < resultado.TotalFilas;
            
            return resultado;
        }
        catch (Exception ex)
        {
            resultado.Exitoso = false;
            resultado.Mensaje = $"Error al importar archivo: {ex.Message}";
            resultado.Errores.Add(new ErrorLog
            {
                Fila = 0,
                Campo = "General",
                Valor = "",
                Error = ex.Message,
                TipoEntidad = "Sistema"
            });
            return resultado;
        }
    }

    public async Task<List<DatosDesnormalizados>> LeerDatosExcelAsync(Stream archivoExcel)
    {
        var datos = new List<DatosDesnormalizados>();
        
        using (var package = new ExcelPackage(archivoExcel))
        {
            var worksheet = package.Workbook.Worksheets[0]; // Primera hoja
            var rowCount = worksheet.Dimension?.Rows ?? 0;
            var colCount = worksheet.Dimension?.Columns ?? 0;

            if (rowCount < 2) // Sin datos (solo encabezados o vacío)
                return datos;

            // Leer encabezados (fila 1)
            var headers = new Dictionary<int, string>();
            for (int col = 1; col <= colCount; col++)
            {
                var headerValue = worksheet.Cells[1, col].Value?.ToString()?.Trim().ToLower() ?? "";
                if (!string.IsNullOrEmpty(headerValue))
                {
                    headers[col] = headerValue;
                }
            }

            // Leer datos (desde fila 2)
            for (int row = 2; row <= rowCount; row++)
            {
                var dato = new DatosDesnormalizados { NumeroFila = row };
                bool tieneAlgunDato = false;

                for (int col = 1; col <= colCount; col++)
                {
                    if (!headers.ContainsKey(col))
                        continue;

                    var header = headers[col];
                    var cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();

                    if (string.IsNullOrWhiteSpace(cellValue))
                        continue;

                    tieneAlgunDato = true;

                    // Mapear columnas a propiedades según el encabezado
                    switch (header)
                    {
                        // Producto
                        case "codigo":
                        case "codigoproducto":
                        case "sku":
                            dato.CodigoProducto = cellValue;
                            dato.TieneDatosProducto = true;
                            break;
                        
                        case "producto":
                        case "nombreproducto":
                        case "nombre":
                            dato.NombreProducto = cellValue;
                            dato.TieneDatosProducto = true;
                            break;
                        
                        case "descripcion":
                        case "descripcionproducto":
                            dato.DescripcionProducto = cellValue;
                            dato.TieneDatosProducto = true;
                            break;
                        
                        case "precio":
                        case "precioproducto":
                        case "preciounitario":
                            if (decimal.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
                            {
                                dato.PrecioProducto = precio;
                                dato.TieneDatosProducto = true;
                            }
                            break;
                        
                        case "stock":
                        case "cantidad":
                        case "existencia":
                            if (int.TryParse(cellValue, out var stock))
                            {
                                dato.StockProducto = stock;
                                dato.TieneDatosProducto = true;
                            }
                            break;
                        
                        case "categoria":
                        case "categoriaproducto":
                            dato.CategoriaProducto = cellValue;
                            dato.TieneDatosProducto = true;
                            break;

                        // Cliente
                        case "codigocliente":
                        case "idcliente":
                            dato.CodigoCliente = cellValue;
                            dato.TieneDatosCliente = true;
                            break;
                        
                        case "cliente":
                        case "nombrecliente":
                            dato.NombreCliente = cellValue;
                            dato.TieneDatosCliente = true;
                            break;
                        
                        case "apellido":
                        case "apellidocliente":
                            dato.ApellidoCliente = cellValue;
                            dato.TieneDatosCliente = true;
                            break;
                        
                        case "email":
                        case "correo":
                        case "emailcliente":
                            dato.EmailCliente = cellValue;
                            dato.TieneDatosCliente = true;
                            break;
                        
                        case "telefono":
                        case "telefonocliente":
                        case "celular":
                            dato.TelefonoCliente = cellValue;
                            dato.TieneDatosCliente = true;
                            break;
                        
                        case "direccion":
                        case "direccioncliente":
                            dato.DireccionCliente = cellValue;
                            dato.TieneDatosCliente = true;
                            break;
                        
                        case "documento":
                        case "dni":
                        case "cedula":
                        case "rut":
                            dato.DocumentoCliente = cellValue;
                            dato.TieneDatosCliente = true;
                            break;

                        // Venta
                        case "factura":
                        case "numerofactura":
                        case "nrofactura":
                            dato.NumeroFactura = cellValue;
                            dato.TieneDatosVenta = true;
                            break;
                        
                        case "fecha":
                        case "fechaventa":
                            if (DateTime.TryParse(cellValue, out var fecha))
                            {
                                dato.FechaVenta = fecha;
                                dato.TieneDatosVenta = true;
                            }
                            break;
                        
                        case "cantidadvendida":
                        case "cantidadventa":
                        case "unidades":
                            if (int.TryParse(cellValue, out var cantidadVenta))
                            {
                                dato.CantidadVendida = cantidadVenta;
                                dato.TieneDatosVenta = true;
                            }
                            break;
                        
                        case "precioventa":
                        case "preciounidad":
                            if (decimal.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var precioVenta))
                            {
                                dato.PrecioUnitarioVenta = precioVenta;
                                dato.TieneDatosVenta = true;
                            }
                            break;
                        
                        case "metodopago":
                        case "pago":
                        case "formapago":
                            dato.MetodoPago = cellValue;
                            dato.TieneDatosVenta = true;
                            break;
                        
                        case "estado":
                        case "estadoventa":
                            dato.EstadoVenta = cellValue;
                            dato.TieneDatosVenta = true;
                            break;
                    }
                }

                if (tieneAlgunDato)
                {
                    datos.Add(dato);
                }
            }
        }

        return datos;
    }

    public async Task<ImportResultado> NormalizarYGuardarDatosAsync(List<DatosDesnormalizados> datos)
    {
        var resultado = new ImportResultado();
        
        // Diccionarios para evitar duplicados
        var productosCache = new Dictionary<string, Producto>();
        var clientesCache = new Dictionary<string, Cliente>();
        var categoriasCache = new Dictionary<string, Categoria>();

        // Cargar datos existentes
        var productosExistentes = await _productoRepository.GetAllAsync();
        var clientesExistentes = await _clienteRepository.GetAllAsync();
        var categoriasExistentes = await _categoriaRepository.GetAllAsync();

        foreach (var productoExistente in productosExistentes)
        {
            if (!string.IsNullOrEmpty(productoExistente.Nombre))
                productosCache[productoExistente.Nombre.ToLower()] = productoExistente;
        }

        foreach (var clienteExistente in clientesExistentes)
        {
            if (!string.IsNullOrEmpty(clienteExistente.Email))
                clientesCache[clienteExistente.Email.ToLower()] = clienteExistente;
        }

        foreach (var categoriaExistente in categoriasExistentes)
        {
            categoriasCache[categoriaExistente.Nombre.ToLower()] = categoriaExistente;
        }

        foreach (var dato in datos)
        {
            try
            {
                // 1. Procesar Categoría (si hay datos de producto)
                Categoria categoria = null;
                if (dato.TieneDatosProducto && !string.IsNullOrEmpty(dato.CategoriaProducto))
                {
                    var categoriaKey = dato.CategoriaProducto.ToLower();
                    
                    if (!categoriasCache.ContainsKey(categoriaKey))
                    {
                        // Crear nueva categoría
                        categoria = new Categoria
                        {
                            Nombre = dato.CategoriaProducto,
                            Descripcion = $"Categoría creada desde importación"
                        };
                        await _categoriaRepository.AddAsync(categoria);
                        categoriasCache[categoriaKey] = categoria;
                    }
                    else
                    {
                        categoria = categoriasCache[categoriaKey];
                    }
                }

                // 2. Procesar Producto
                if (dato.TieneDatosProducto && !string.IsNullOrWhiteSpace(dato.NombreProducto))
                {
                    var nombreProducto = dato.NombreProducto.Trim();
                    var productoKey = nombreProducto.ToLower();
                    
                    if (productosCache.ContainsKey(productoKey))
                    {
                        // Actualizar producto existente (solo campos proporcionados)
                        var producto = productosCache[productoKey];
                        
                        if (!string.IsNullOrWhiteSpace(dato.DescripcionProducto))
                            producto.Descripcion = dato.DescripcionProducto;
                        
                        if (dato.PrecioProducto.HasValue && dato.PrecioProducto.Value > 0)
                            producto.Precio = dato.PrecioProducto.Value;
                        
                        if (dato.StockProducto.HasValue)
                            producto.Stock = dato.StockProducto.Value;
                        
                        if (categoria != null)
                            producto.CategoriaId = categoria.Id;

                        await _productoRepository.UpdateAsync(producto);
                        resultado.ProductosActualizados++;
                    }
                    else
                    {
                        // Crear nuevo producto - Validar campos obligatorios
                        if (!dato.PrecioProducto.HasValue || dato.PrecioProducto.Value <= 0)
                        {
                            resultado.FilasConError++;
                            resultado.Errores.Add(new ErrorLog
                            {
                                Fila = dato.NumeroFila,
                                Campo = "PrecioProducto",
                                Valor = dato.PrecioProducto?.ToString() ?? "null",
                                Error = "El precio es obligatorio para crear un nuevo producto",
                                TipoEntidad = "Producto"
                            });
                            continue;
                        }
                        
                        var nuevoProducto = new Producto
                        {
                            Nombre = nombreProducto,
                            Descripcion = dato.DescripcionProducto ?? "",
                            Precio = dato.PrecioProducto.Value,
                            Stock = dato.StockProducto ?? 0,
                            CategoriaId = categoria?.Id ?? 1
                        };
                        await _productoRepository.AddAsync(nuevoProducto);
                        productosCache[productoKey] = nuevoProducto;
                        resultado.ProductosCreados++;
                    }
                }

                // 3. Procesar Cliente
                if (dato.TieneDatosCliente)
                {
                    // Validación flexible - necesita al menos nombre o email
                    if (string.IsNullOrWhiteSpace(dato.NombreCliente) && string.IsNullOrWhiteSpace(dato.EmailCliente))
                    {
                        resultado.FilasConError++;
                        resultado.Errores.Add(new ErrorLog
                        {
                            Fila = dato.NumeroFila,
                            Campo = "Cliente",
                            Valor = "",
                            Error = "Debe proporcionar al menos el nombre o email del cliente",
                            TipoEntidad = "Cliente"
                        });
                        continue;
                    }

                    var emailKey = dato.EmailCliente?.ToLower()?.Trim() ?? "";
                    
                    if (!string.IsNullOrEmpty(emailKey) && clientesCache.ContainsKey(emailKey))
                    {
                        // Actualizar cliente existente (solo campos proporcionados)
                        var cliente = clientesCache[emailKey];
                        
                        if (!string.IsNullOrWhiteSpace(dato.NombreCliente))
                            cliente.Nombre = dato.NombreCliente.Trim();
                        
                        if (!string.IsNullOrWhiteSpace(dato.ApellidoCliente))
                            cliente.Apellido = dato.ApellidoCliente.Trim();
                        
                        if (!string.IsNullOrWhiteSpace(dato.TelefonoCliente))
                            cliente.Telefono = dato.TelefonoCliente.Trim();
                        
                        if (!string.IsNullOrWhiteSpace(dato.DireccionCliente))
                            cliente.Direccion = dato.DireccionCliente.Trim();
                        
                        if (!string.IsNullOrWhiteSpace(dato.DocumentoCliente))
                            cliente.Documento = dato.DocumentoCliente.Trim();

                        await _clienteRepository.UpdateAsync(cliente);
                        resultado.ClientesActualizados++;
                    }
                    else
                    {
                        // Crear nuevo cliente
                        var nuevoCliente = new Cliente
                        {
                            Nombre = dato.NombreCliente?.Trim() ?? "Cliente Importado",
                            Apellido = dato.ApellidoCliente?.Trim() ?? "",
                            Email = !string.IsNullOrWhiteSpace(emailKey) ? emailKey : $"cliente{Guid.NewGuid().ToString().Substring(0, 8)}@temporal.com",
                            Telefono = dato.TelefonoCliente?.Trim() ?? "",
                            Direccion = dato.DireccionCliente?.Trim() ?? "",
                            Documento = dato.DocumentoCliente?.Trim() ?? "",
                            Activo = true,
                            FechaRegistro = DateTime.UtcNow
                        };
                        await _clienteRepository.AddAsync(nuevoCliente);
                        
                        if (!string.IsNullOrEmpty(emailKey))
                            clientesCache[emailKey] = nuevoCliente;
                        
                        resultado.ClientesCreados++;
                    }
                }

                // 4. Procesar Venta (si tiene todos los datos necesarios)
                if (dato.TieneDatosVenta && dato.TieneDatosCliente && dato.TieneDatosProducto)
                {
                    var erroresVenta = ValidarVenta(dato);
                    if (erroresVenta.Any())
                    {
                        resultado.FilasConError++;
                        resultado.Errores.AddRange(erroresVenta);
                        continue;
                    }

                    // Buscar producto y cliente
                    var productoKey = dato.NombreProducto?.ToLower() ?? "";
                    var clienteKey = dato.EmailCliente?.ToLower() ?? "";

                    if (productosCache.ContainsKey(productoKey) && clientesCache.ContainsKey(clienteKey))
                    {
                        var producto = productosCache[productoKey];
                        var cliente = clientesCache[clienteKey];

                        var nuevaVenta = new Venta
                        {
                            NumeroFactura = dato.NumeroFactura ?? Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                            FechaVenta = dato.FechaVenta ?? DateTime.UtcNow,
                            Cliente = cliente.NombreCompleto,
                            ClienteId = cliente.Id,
                            MetodoPago = dato.MetodoPago ?? "Efectivo",
                            Estado = dato.EstadoVenta ?? "Completada",
                            Vendedor = "Sistema - Importación",
                            Detalles = new List<DetalleDeVenta>
                            {
                                new DetalleDeVenta
                                {
                                    ProductoId = producto.Id,
                                    Cantidad = dato.CantidadVendida ?? 1,
                                    PrecioUnitario = dato.PrecioUnitarioVenta ?? producto.Precio
                                }
                            }
                        };

                        // Calcular totales
                        foreach (var detalle in nuevaVenta.Detalles)
                        {
                            detalle.CalcularSubtotal();
                        }
                        
                        nuevaVenta.Subtotal = nuevaVenta.Detalles.Sum(d => d.Subtotal);
                        nuevaVenta.IVA = nuevaVenta.Subtotal * 0.16m;
                        nuevaVenta.Total = nuevaVenta.Subtotal + nuevaVenta.IVA;

                        await _ventaRepository.AddAsync(nuevaVenta);
                        
                        // Actualizar stock
                        producto.Stock -= (dato.CantidadVendida ?? 1);
                        await _productoRepository.UpdateAsync(producto);
                        
                        resultado.VentasCreadas++;
                    }
                }

                resultado.FilasExitosas++;
            }
            catch (Exception ex)
            {
                resultado.FilasConError++;
                resultado.Errores.Add(new ErrorLog
                {
                    Fila = dato.NumeroFila,
                    Campo = "General",
                    Valor = "",
                    Error = ex.Message,
                    TipoEntidad = "Procesamiento"
                });
            }
        }

        resultado.Mensaje = $"Importación completada: {resultado.FilasExitosas} exitosas, {resultado.FilasConError} con errores.";
        return resultado;
    }

    private List<ErrorLog> ValidarProducto(DatosDesnormalizados dato)
    {
        var errores = new List<ErrorLog>();

        // Solo validar nombre (obligatorio siempre)
        if (string.IsNullOrWhiteSpace(dato.NombreProducto))
        {
            errores.Add(new ErrorLog
            {
                Fila = dato.NumeroFila,
                Campo = "NombreProducto",
                Valor = dato.NombreProducto ?? "",
                Error = "El nombre del producto es obligatorio",
                TipoEntidad = "Producto"
            });
        }

        // Validar precio solo si se está creando un nuevo producto (no existe en cache)
        // El precio puede no estar si solo se actualiza stock o descripción
        
        return errores;
    }

    private List<ErrorLog> ValidarCliente(DatosDesnormalizados dato)
    {
        var errores = new List<ErrorLog>();

        // Validar que al menos tenga nombre o email
        if (string.IsNullOrWhiteSpace(dato.NombreCliente) && string.IsNullOrWhiteSpace(dato.EmailCliente))
        {
            errores.Add(new ErrorLog
            {
                Fila = dato.NumeroFila,
                Campo = "Cliente",
                Valor = "",
                Error = "Debe proporcionar al menos el nombre o email del cliente",
                TipoEntidad = "Cliente"
            });
        }

        return errores;
    }

    private List<ErrorLog> ValidarVenta(DatosDesnormalizados dato)
    {
        var errores = new List<ErrorLog>();

        if (!dato.CantidadVendida.HasValue || dato.CantidadVendida <= 0)
        {
            errores.Add(new ErrorLog
            {
                Fila = dato.NumeroFila,
                Campo = "CantidadVendida",
                Valor = dato.CantidadVendida?.ToString() ?? "null",
                Error = "La cantidad vendida debe ser mayor a 0",
                TipoEntidad = "Venta"
            });
        }

        return errores;
    }

    public async Task<byte[]> GenerarPlantillaExcelAsync(string tipoPlantilla)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Plantilla");

            switch (tipoPlantilla.ToLower())
            {
                case "productos":
                    GenerarPlantillaProductos(worksheet);
                    break;
                case "clientes":
                    GenerarPlantillaClientes(worksheet);
                    break;
                case "ventas":
                    GenerarPlantillaVentas(worksheet);
                    break;
                default: // "completa" o cualquier otro
                    GenerarPlantillaCompleta(worksheet);
                    break;
            }

            return await package.GetAsByteArrayAsync();
        }
    }

    private void GenerarPlantillaProductos(ExcelWorksheet worksheet)
    {
        worksheet.Cells[1, 1].Value = "Codigo";
        worksheet.Cells[1, 2].Value = "NombreProducto";
        worksheet.Cells[1, 3].Value = "Descripcion";
        worksheet.Cells[1, 4].Value = "Precio";
        worksheet.Cells[1, 5].Value = "Stock";
        worksheet.Cells[1, 6].Value = "Categoria";

        // Formato de encabezados
        using (var range = worksheet.Cells[1, 1, 1, 6])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        }

        // Ejemplo
        worksheet.Cells[2, 1].Value = "PROD001";
        worksheet.Cells[2, 2].Value = "Laptop Dell";
        worksheet.Cells[2, 3].Value = "Laptop Dell Inspiron 15";
        worksheet.Cells[2, 4].Value = 899.99;
        worksheet.Cells[2, 5].Value = 10;
        worksheet.Cells[2, 6].Value = "Tecnología";

        worksheet.Cells.AutoFitColumns();
    }

    private void GenerarPlantillaClientes(ExcelWorksheet worksheet)
    {
        worksheet.Cells[1, 1].Value = "NombreCliente";
        worksheet.Cells[1, 2].Value = "Apellido";
        worksheet.Cells[1, 3].Value = "Email";
        worksheet.Cells[1, 4].Value = "Telefono";
        worksheet.Cells[1, 5].Value = "Direccion";
        worksheet.Cells[1, 6].Value = "Documento";

        using (var range = worksheet.Cells[1, 1, 1, 6])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
        }

        // Ejemplo
        worksheet.Cells[2, 1].Value = "Juan";
        worksheet.Cells[2, 2].Value = "Pérez";
        worksheet.Cells[2, 3].Value = "juan.perez@example.com";
        worksheet.Cells[2, 4].Value = "555-1234";
        worksheet.Cells[2, 5].Value = "Calle Principal 123";
        worksheet.Cells[2, 6].Value = "12345678";

        worksheet.Cells.AutoFitColumns();
    }

    private void GenerarPlantillaVentas(ExcelWorksheet worksheet)
    {
        worksheet.Cells[1, 1].Value = "CodigoProducto";
        worksheet.Cells[1, 2].Value = "EmailCliente";
        worksheet.Cells[1, 3].Value = "CantidadVendida";
        worksheet.Cells[1, 4].Value = "PrecioVenta";
        worksheet.Cells[1, 5].Value = "Fecha";
        worksheet.Cells[1, 6].Value = "MetodoPago";

        using (var range = worksheet.Cells[1, 1, 1, 6])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
        }

        worksheet.Cells.AutoFitColumns();
    }

    private void GenerarPlantillaCompleta(ExcelWorksheet worksheet)
    {
        // Encabezados para datos desnormalizados completos
        var headers = new[]
        {
            "Codigo", "NombreProducto", "Descripcion", "Precio", "Stock", "Categoria",
            "NombreCliente", "Apellido", "Email", "Telefono", "Direccion", "Documento",
            "CantidadVendida", "PrecioVenta", "Fecha", "MetodoPago"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[1, i + 1].Value = headers[i];
        }

        using (var range = worksheet.Cells[1, 1, 1, headers.Length])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
        }

        // Ejemplo completo
        worksheet.Cells[2, 1].Value = "PROD001";
        worksheet.Cells[2, 2].Value = "Laptop Dell";
        worksheet.Cells[2, 3].Value = "Laptop Dell Inspiron 15";
        worksheet.Cells[2, 4].Value = 899.99;
        worksheet.Cells[2, 5].Value = 10;
        worksheet.Cells[2, 6].Value = "Tecnología";
        worksheet.Cells[2, 7].Value = "Juan";
        worksheet.Cells[2, 8].Value = "Pérez";
        worksheet.Cells[2, 9].Value = "juan.perez@example.com";
        worksheet.Cells[2, 10].Value = "555-1234";
        worksheet.Cells[2, 11].Value = "Calle Principal 123";
        worksheet.Cells[2, 12].Value = "12345678";
        worksheet.Cells[2, 13].Value = 2;
        worksheet.Cells[2, 14].Value = 899.99;
        worksheet.Cells[2, 15].Value = DateTime.Now.ToString("yyyy-MM-dd");
        worksheet.Cells[2, 16].Value = "Tarjeta";

        worksheet.Cells.AutoFitColumns();
    }
}

