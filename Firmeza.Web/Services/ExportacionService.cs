using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using DrawingColor = System.Drawing.Color;

namespace Firmeza.Web.Services;

public class ExportacionService : IExportacionService
{
    private readonly IProductoRepository _productoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IVentaRepository _ventaRepository;

    public ExportacionService(
        IProductoRepository productoRepository,
        IClienteRepository clienteRepository,
        IVentaRepository ventaRepository)
    {
        _productoRepository = productoRepository;
        _clienteRepository = clienteRepository;
        _ventaRepository = ventaRepository;
        
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    #region Exportación a Excel

    public async Task<byte[]> ExportarProductosExcelAsync()
    {
        var productos = await _productoRepository.GetAllAsync();
        
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Productos");

        // Encabezados
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Nombre";
        worksheet.Cells[1, 3].Value = "Descripción";
        worksheet.Cells[1, 4].Value = "Precio";
        worksheet.Cells[1, 5].Value = "Stock";
        worksheet.Cells[1, 6].Value = "Categoría";

        // Formato de encabezados
        using (var range = worksheet.Cells[1, 1, 1, 6])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(DrawingColor.FromArgb(79, 129, 189));
            range.Style.Font.Color.SetColor(DrawingColor.White);
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // Datos
        int row = 2;
        foreach (var producto in productos)
        {
            worksheet.Cells[row, 1].Value = producto.Id;
            worksheet.Cells[row, 2].Value = producto.Nombre;
            worksheet.Cells[row, 3].Value = producto.Descripcion;
            worksheet.Cells[row, 4].Value = producto.Precio;
            worksheet.Cells[row, 5].Value = producto.Stock;
            worksheet.Cells[row, 6].Value = producto.Categoria?.Nombre ?? "Sin categoría";
            
            // Formato de precio
            worksheet.Cells[row, 4].Style.Numberformat.Format = "$#,##0.00";
            
            row++;
        }

        worksheet.Cells.AutoFitColumns();
        
        return await package.GetAsByteArrayAsync();
    }

    public async Task<byte[]> ExportarClientesExcelAsync()
    {
        var clientes = await _clienteRepository.GetAllAsync();
        
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Clientes");

        // Encabezados
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Nombre";
        worksheet.Cells[1, 3].Value = "Apellido";
        worksheet.Cells[1, 4].Value = "Email";
        worksheet.Cells[1, 5].Value = "Teléfono";
        worksheet.Cells[1, 6].Value = "Dirección";
        worksheet.Cells[1, 7].Value = "Documento";
        worksheet.Cells[1, 8].Value = "Fecha Registro";
        worksheet.Cells[1, 9].Value = "Estado";

        // Formato de encabezados
        using (var range = worksheet.Cells[1, 1, 1, 9])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(DrawingColor.FromArgb(155, 194, 230));
            range.Style.Font.Color.SetColor(DrawingColor.Black);
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // Datos
        int row = 2;
        foreach (var cliente in clientes)
        {
            worksheet.Cells[row, 1].Value = cliente.Id;
            worksheet.Cells[row, 2].Value = cliente.Nombre;
            worksheet.Cells[row, 3].Value = cliente.Apellido;
            worksheet.Cells[row, 4].Value = cliente.Email;
            worksheet.Cells[row, 5].Value = cliente.Telefono;
            worksheet.Cells[row, 6].Value = cliente.Direccion;
            worksheet.Cells[row, 7].Value = cliente.Documento;
            worksheet.Cells[row, 8].Value = cliente.FechaRegistro;
            worksheet.Cells[row, 9].Value = cliente.Activo ? "Activo" : "Inactivo";
            
            // Formato de fecha
            worksheet.Cells[row, 8].Style.Numberformat.Format = "dd/mm/yyyy";
            
            row++;
        }

        worksheet.Cells.AutoFitColumns();
        
        return await package.GetAsByteArrayAsync();
    }

    public async Task<byte[]> ExportarVentasExcelAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null)
    {
        var ventasQuery = await _ventaRepository.GetAllAsync();
        
        // Filtrar por fechas si se proporcionan
        if (fechaInicio.HasValue)
        {
            ventasQuery = ventasQuery.Where(v => v.FechaVenta >= fechaInicio.Value).ToList();
        }
        
        if (fechaFin.HasValue)
        {
            ventasQuery = ventasQuery.Where(v => v.FechaVenta <= fechaFin.Value).ToList();
        }
        
        var ventas = ventasQuery.ToList();
        
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Ventas");

        // Encabezados
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Número Factura";
        worksheet.Cells[1, 3].Value = "Fecha";
        worksheet.Cells[1, 4].Value = "Cliente";
        worksheet.Cells[1, 5].Value = "Subtotal";
        worksheet.Cells[1, 6].Value = "IVA";
        worksheet.Cells[1, 7].Value = "Total";
        worksheet.Cells[1, 8].Value = "Método Pago";
        worksheet.Cells[1, 9].Value = "Estado";
        worksheet.Cells[1, 10].Value = "Vendedor";

        // Formato de encabezados
        using (var range = worksheet.Cells[1, 1, 1, 10])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(DrawingColor.FromArgb(146, 208, 80));
            range.Style.Font.Color.SetColor(DrawingColor.Black);
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // Datos
        int row = 2;
        foreach (var venta in ventas)
        {
            worksheet.Cells[row, 1].Value = venta.Id;
            worksheet.Cells[row, 2].Value = venta.NumeroFactura;
            worksheet.Cells[row, 3].Value = venta.FechaVenta;
            worksheet.Cells[row, 4].Value = venta.Cliente;
            worksheet.Cells[row, 5].Value = venta.Subtotal;
            worksheet.Cells[row, 6].Value = venta.IVA;
            worksheet.Cells[row, 7].Value = venta.Total;
            worksheet.Cells[row, 8].Value = venta.MetodoPago;
            worksheet.Cells[row, 9].Value = venta.Estado;
            worksheet.Cells[row, 10].Value = venta.Vendedor;
            
            // Formato de fecha y moneda
            worksheet.Cells[row, 3].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
            worksheet.Cells[row, 5].Style.Numberformat.Format = "$#,##0.00";
            worksheet.Cells[row, 6].Style.Numberformat.Format = "$#,##0.00";
            worksheet.Cells[row, 7].Style.Numberformat.Format = "$#,##0.00";
            
            row++;
        }

        // Totales
        if (ventas.Any())
        {
            row++;
            worksheet.Cells[row, 4].Value = "TOTALES:";
            worksheet.Cells[row, 5].Formula = $"SUM(E2:E{row - 1})";
            worksheet.Cells[row, 6].Formula = $"SUM(F2:F{row - 1})";
            worksheet.Cells[row, 7].Formula = $"SUM(G2:G{row - 1})";
            
            using (var range = worksheet.Cells[row, 4, row, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(DrawingColor.LightYellow);
            }
            
            worksheet.Cells[row, 5].Style.Numberformat.Format = "$#,##0.00";
            worksheet.Cells[row, 6].Style.Numberformat.Format = "$#,##0.00";
            worksheet.Cells[row, 7].Style.Numberformat.Format = "$#,##0.00";
        }

        worksheet.Cells.AutoFitColumns();
        
        return await package.GetAsByteArrayAsync();
    }

    #endregion

    #region Exportación a PDF

    public async Task<byte[]> ExportarProductosPdfAsync()
    {
        var productos = await _productoRepository.GetAllAsync();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(container =>
                {
                    container.Column(column =>
                    {
                        column.Item().Text("LISTADO DE PRODUCTOS")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);
                        
                        column.Item().Text($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);
                        
                        column.Item().PaddingVertical(5).LineHorizontal(1);
                    });
                });

                page.Content().Element(container =>
                {
                    container.Table(table =>
                    {
                        // Definir columnas
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);  // ID
                            columns.RelativeColumn(3);    // Nombre
                            columns.RelativeColumn(3);    // Descripción
                            columns.RelativeColumn(1.5f); // Precio
                            columns.RelativeColumn(1);    // Stock
                            columns.RelativeColumn(2);    // Categoría
                        });

                        // Encabezados
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID").Bold();
                            header.Cell().Element(CellStyle).Text("Nombre").Bold();
                            header.Cell().Element(CellStyle).Text("Descripción").Bold();
                            header.Cell().Element(CellStyle).Text("Precio").Bold();
                            header.Cell().Element(CellStyle).Text("Stock").Bold();
                            header.Cell().Element(CellStyle).Text("Categoría").Bold();

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .Border(1)
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(5);
                            }
                        });

                        // Datos
                        foreach (var producto in productos)
                        {
                            table.Cell().Element(CellStyle).Text(producto.Id.ToString());
                            table.Cell().Element(CellStyle).Text(producto.Nombre);
                            table.Cell().Element(CellStyle).Text(producto.Descripcion ?? "");
                            table.Cell().Element(CellStyle).Text($"${producto.Precio:N2}");
                            table.Cell().Element(CellStyle).Text(producto.Stock.ToString());
                            table.Cell().Element(CellStyle).Text(producto.Categoria?.Nombre ?? "Sin categoría");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Lighten1)
                                    .Padding(5);
                            }
                        }
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    public async Task<byte[]> ExportarClientesPdfAsync()
    {
        var clientes = await _clienteRepository.GetAllAsync();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header().Element(container =>
                {
                    container.Column(column =>
                    {
                        column.Item().Text("LISTADO DE CLIENTES")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);
                        
                        column.Item().Text($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);
                        
                        column.Item().PaddingVertical(5).LineHorizontal(1);
                    });
                });

                page.Content().Element(container =>
                {
                    container.Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);  // ID
                            columns.RelativeColumn(2);   // Nombre
                            columns.RelativeColumn(2);   // Apellido
                            columns.RelativeColumn(3);   // Email
                            columns.RelativeColumn(2);   // Teléfono
                            columns.RelativeColumn(3);   // Dirección
                            columns.RelativeColumn(1.5f);// Documento
                            columns.RelativeColumn(1);   // Estado
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID").Bold();
                            header.Cell().Element(CellStyle).Text("Nombre").Bold();
                            header.Cell().Element(CellStyle).Text("Apellido").Bold();
                            header.Cell().Element(CellStyle).Text("Email").Bold();
                            header.Cell().Element(CellStyle).Text("Teléfono").Bold();
                            header.Cell().Element(CellStyle).Text("Dirección").Bold();
                            header.Cell().Element(CellStyle).Text("Documento").Bold();
                            header.Cell().Element(CellStyle).Text("Estado").Bold();

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .Border(1)
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(5);
                            }
                        });

                        foreach (var cliente in clientes)
                        {
                            table.Cell().Element(CellStyle).Text(cliente.Id.ToString());
                            table.Cell().Element(CellStyle).Text(cliente.Nombre);
                            table.Cell().Element(CellStyle).Text(cliente.Apellido ?? "");
                            table.Cell().Element(CellStyle).Text(cliente.Email);
                            table.Cell().Element(CellStyle).Text(cliente.Telefono ?? "");
                            table.Cell().Element(CellStyle).Text(cliente.Direccion ?? "");
                            table.Cell().Element(CellStyle).Text(cliente.Documento ?? "");
                            table.Cell().Element(CellStyle).Text(cliente.Activo ? "Activo" : "Inactivo");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Lighten1)
                                    .Padding(5);
                            }
                        }
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    public async Task<byte[]> ExportarVentasPdfAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null)
    {
        var ventasQuery = await _ventaRepository.GetAllAsync();
        
        if (fechaInicio.HasValue)
        {
            ventasQuery = ventasQuery.Where(v => v.FechaVenta >= fechaInicio.Value).ToList();
        }
        
        if (fechaFin.HasValue)
        {
            ventasQuery = ventasQuery.Where(v => v.FechaVenta <= fechaFin.Value).ToList();
        }
        
        var ventas = ventasQuery.ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header().Element(container =>
                {
                    container.Column(column =>
                    {
                        column.Item().Text("REPORTE DE VENTAS")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);
                        
                        if (fechaInicio.HasValue || fechaFin.HasValue)
                        {
                            var periodo = $"Período: {fechaInicio?.ToString("dd/MM/yyyy") ?? "Inicio"} - {fechaFin?.ToString("dd/MM/yyyy") ?? "Fin"}";
                            column.Item().Text(periodo).FontSize(11);
                        }
                        
                        column.Item().Text($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);
                        
                        column.Item().PaddingVertical(5).LineHorizontal(1);
                    });
                });

                page.Content().Element(container =>
                {
                    container.Column(column =>
                    {
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);  // ID
                                columns.RelativeColumn(1.5f);// Factura
                                columns.RelativeColumn(2);   // Fecha
                                columns.RelativeColumn(3);   // Cliente
                                columns.RelativeColumn(1.5f);// Subtotal
                                columns.RelativeColumn(1.5f);// IVA
                                columns.RelativeColumn(1.5f);// Total
                                columns.RelativeColumn(1.5f);// Método
                                columns.RelativeColumn(1.5f);// Estado
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("ID").Bold();
                                header.Cell().Element(CellStyle).Text("Factura").Bold();
                                header.Cell().Element(CellStyle).Text("Fecha").Bold();
                                header.Cell().Element(CellStyle).Text("Cliente").Bold();
                                header.Cell().Element(CellStyle).Text("Subtotal").Bold();
                                header.Cell().Element(CellStyle).Text("IVA").Bold();
                                header.Cell().Element(CellStyle).Text("Total").Bold();
                                header.Cell().Element(CellStyle).Text("Método").Bold();
                                header.Cell().Element(CellStyle).Text("Estado").Bold();

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container
                                        .Border(1)
                                        .Background(Colors.Grey.Lighten2)
                                        .Padding(5);
                                }
                            });

                            foreach (var venta in ventas)
                            {
                                table.Cell().Element(CellStyle).Text(venta.Id.ToString());
                                table.Cell().Element(CellStyle).Text(venta.NumeroFactura);
                                table.Cell().Element(CellStyle).Text(venta.FechaVenta.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(venta.Cliente);
                                table.Cell().Element(CellStyle).Text($"${venta.Subtotal:N2}");
                                table.Cell().Element(CellStyle).Text($"${venta.IVA:N2}");
                                table.Cell().Element(CellStyle).Text($"${venta.Total:N2}");
                                table.Cell().Element(CellStyle).Text(venta.MetodoPago);
                                table.Cell().Element(CellStyle).Text(venta.Estado);

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container
                                        .Border(1)
                                        .BorderColor(Colors.Grey.Lighten1)
                                        .Padding(5);
                                }
                            }
                        });

                        // Totales
                        if (ventas.Any())
                        {
                            column.Item().PaddingTop(10).AlignRight().Text(text =>
                            {
                                text.Span("Total Subtotal: ").Bold();
                                text.Span($"${ventas.Sum(v => v.Subtotal):N2}").FontColor(Colors.Green.Darken2);
                                text.Span(" | ");
                                text.Span("Total IVA: ").Bold();
                                text.Span($"${ventas.Sum(v => v.IVA):N2}").FontColor(Colors.Green.Darken2);
                                text.Span(" | ");
                                text.Span("TOTAL: ").Bold();
                                text.Span($"${ventas.Sum(v => v.Total):N2}").FontSize(14).FontColor(Colors.Green.Darken3);
                            });
                        }
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    #endregion
}

