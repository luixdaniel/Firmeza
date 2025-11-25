using Firmeza.Web.Data.Entities;
using Firmeza.Web.Interfaces.Repositories;
using Firmeza.Web.Interfaces.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Firmeza.Web.Services;

public class PdfService : IPdfService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IWebHostEnvironment _environment;
    private readonly string _recibosPath;

    public PdfService(IVentaRepository ventaRepository, IWebHostEnvironment environment)
    {
        _ventaRepository = ventaRepository;
        _environment = environment;
        
        // Si WebRootPath es null (como en APIs), usar ContentRootPath
        var basePath = _environment.WebRootPath ?? _environment.ContentRootPath;
        _recibosPath = Path.Combine(basePath, "recibos");
        
        // Crear directorio si no existe
        if (!Directory.Exists(_recibosPath))
        {
            Directory.CreateDirectory(_recibosPath);
        }
        
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<string> GenerarReciboPdfAsync(Venta venta)
    {
        // Cargar la venta con sus detalles si no los tiene
        if (venta.Detalles == null || !venta.Detalles.Any())
        {
            venta = await _ventaRepository.GetByIdAsync(venta.Id) ?? venta;
        }

        var nombreArchivo = $"Recibo_{venta.NumeroFactura}_{venta.Id}.pdf";
        var rutaCompleta = Path.Combine(_recibosPath, nombreArchivo);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Element(container =>
                {
                    container.Column(column =>
                    {
                        // Logo y título de la empresa
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("FIRMEZA")
                                    .FontSize(24)
                                    .Bold()
                                    .FontColor(Colors.Blue.Darken3);
                                
                                col.Item().Text("Sistema de Gestión de Ventas")
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken2);
                            });

                            row.RelativeItem().AlignRight().Column(col =>
                            {
                                col.Item().Text("RECIBO DE VENTA")
                                    .FontSize(16)
                                    .Bold();
                                
                                col.Item().Text($"Factura N°: {venta.NumeroFactura}")
                                    .FontSize(12)
                                    .Bold()
                                    .FontColor(Colors.Red.Darken2);
                            });
                        });

                        column.Item().PaddingVertical(10).LineHorizontal(2).LineColor(Colors.Blue.Darken3);
                    });
                });

                page.Content().Element(container =>
                {
                    container.Column(column =>
                    {
                        // Información de la venta
                        column.Item().PaddingBottom(10).Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("DATOS DEL CLIENTE")
                                    .Bold()
                                    .FontSize(12)
                                    .FontColor(Colors.Blue.Darken2);
                                
                                col.Item().PaddingTop(5).Text(text =>
                                {
                                    text.Span("Cliente: ").Bold();
                                    text.Span(venta.Cliente);
                                });

                                if (venta.ClienteEntity != null)
                                {
                                    if (!string.IsNullOrEmpty(venta.ClienteEntity.Email))
                                    {
                                        col.Item().Text(text =>
                                        {
                                            text.Span("Email: ").Bold();
                                            text.Span(venta.ClienteEntity.Email);
                                        });
                                    }

                                    if (!string.IsNullOrEmpty(venta.ClienteEntity.Telefono))
                                    {
                                        col.Item().Text(text =>
                                        {
                                            text.Span("Teléfono: ").Bold();
                                            text.Span(venta.ClienteEntity.Telefono);
                                        });
                                    }

                                    if (!string.IsNullOrEmpty(venta.ClienteEntity.Direccion))
                                    {
                                        col.Item().Text(text =>
                                        {
                                            text.Span("Dirección: ").Bold();
                                            text.Span(venta.ClienteEntity.Direccion);
                                        });
                                    }

                                    if (!string.IsNullOrEmpty(venta.ClienteEntity.Documento))
                                    {
                                        col.Item().Text(text =>
                                        {
                                            text.Span("Documento: ").Bold();
                                            text.Span(venta.ClienteEntity.Documento);
                                        });
                                    }
                                }
                            });

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("DATOS DE LA VENTA")
                                    .Bold()
                                    .FontSize(12)
                                    .FontColor(Colors.Blue.Darken2);
                                
                                col.Item().PaddingTop(5).Text(text =>
                                {
                                    text.Span("Fecha: ").Bold();
                                    text.Span(venta.FechaVenta.ToString("dd/MM/yyyy HH:mm"));
                                });

                                col.Item().Text(text =>
                                {
                                    text.Span("Método de Pago: ").Bold();
                                    text.Span(venta.MetodoPago);
                                });

                                col.Item().Text(text =>
                                {
                                    text.Span("Estado: ").Bold();
                                    text.Span(venta.Estado);
                                });

                                if (!string.IsNullOrEmpty(venta.Vendedor))
                                {
                                    col.Item().Text(text =>
                                    {
                                        text.Span("Vendedor: ").Bold();
                                        text.Span(venta.Vendedor);
                                    });
                                }
                            });
                        });

                        // Línea divisoria
                        column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                        // Detalle de productos
                        column.Item().Text("DETALLE DE PRODUCTOS")
                            .Bold()
                            .FontSize(12)
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            // Definir columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);   // Cant
                                columns.RelativeColumn(4);     // Producto
                                columns.RelativeColumn(2);     // Precio Unit
                                columns.RelativeColumn(2);     // Subtotal
                            });

                            // Encabezados
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderStyle).Text("Cant.").Bold();
                                header.Cell().Element(HeaderStyle).Text("Producto").Bold();
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Precio Unit.").Bold();
                                header.Cell().Element(HeaderStyle).AlignRight().Text("Subtotal").Bold();

                                static IContainer HeaderStyle(IContainer container)
                                {
                                    return container
                                        .Border(1)
                                        .Background(Colors.Blue.Lighten4)
                                        .Padding(8);
                                }
                            });

                            // Productos
                            if (venta.Detalles != null && venta.Detalles.Any())
                            {
                                foreach (var detalle in venta.Detalles)
                                {
                                    table.Cell().Element(CellStyle).Text(detalle.Cantidad.ToString());
                                    table.Cell().Element(CellStyle).Text(detalle.Producto?.Nombre ?? "Producto");
                                    table.Cell().Element(CellStyle).AlignRight().Text($"${detalle.PrecioUnitario:N2}");
                                    table.Cell().Element(CellStyle).AlignRight().Text($"${detalle.Subtotal:N2}");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container
                                            .Border(1)
                                            .BorderColor(Colors.Grey.Lighten1)
                                            .Padding(8);
                                    }
                                }
                            }
                        });

                        // Totales
                        column.Item().PaddingTop(15).AlignRight().Column(col =>
                        {
                            col.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("Subtotal:").Bold().FontSize(12);
                                row.ConstantItem(100).AlignRight().Text($"${venta.Subtotal:N2}").FontSize(12);
                            });

                            col.Item().Row(row =>
                            {
                                row.ConstantItem(150).Text("IVA (16%):").Bold().FontSize(12);
                                row.ConstantItem(100).AlignRight().Text($"${venta.IVA:N2}").FontSize(12);
                            });

                            col.Item().PaddingTop(5).LineHorizontal(2).LineColor(Colors.Blue.Darken3);

                            col.Item().PaddingTop(5).Row(row =>
                            {
                                row.ConstantItem(150).Text("TOTAL:").Bold().FontSize(16).FontColor(Colors.Blue.Darken3);
                                row.ConstantItem(100).AlignRight().Text($"${venta.Total:N2}")
                                    .Bold()
                                    .FontSize(16)
                                    .FontColor(Colors.Green.Darken3);
                            });
                        });

                        // Nota al pie
                        column.Item().PaddingTop(30).Column(col =>
                        {
                            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                            col.Item().PaddingTop(10).Text("Gracias por su compra")
                                .FontSize(10)
                                .Italic()
                                .FontColor(Colors.Grey.Darken1);
                        });
                    });
                });

                page.Footer().AlignCenter().Column(column =>
                {
                    column.Item().Text($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                        .FontSize(8)
                        .FontColor(Colors.Grey.Darken1);

                    column.Item().Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            });
        });

        // Guardar el PDF
        document.GeneratePdf(rutaCompleta);

        // Retornar la ruta relativa
        return $"/recibos/{nombreArchivo}";
    }

    public async Task<byte[]> ObtenerReciboPdfAsync(string nombreArchivo)
    {
        var rutaCompleta = Path.Combine(_recibosPath, nombreArchivo);
        
        if (!File.Exists(rutaCompleta))
        {
            throw new FileNotFoundException("El recibo no existe", nombreArchivo);
        }

        return await File.ReadAllBytesAsync(rutaCompleta);
    }

    public async Task<bool> ExisteReciboPdfAsync(int ventaId)
    {
        var venta = await _ventaRepository.GetByIdAsync(ventaId);
        if (venta == null) return false;

        var nombreArchivo = $"Recibo_{venta.NumeroFactura}_{venta.Id}.pdf";
        var rutaCompleta = Path.Combine(_recibosPath, nombreArchivo);
        
        return File.Exists(rutaCompleta);
    }

    public async Task<string?> ObtenerRutaReciboPdfAsync(int ventaId)
    {
        var venta = await _ventaRepository.GetByIdAsync(ventaId);
        if (venta == null) return null;

        var nombreArchivo = $"Recibo_{venta.NumeroFactura}_{venta.Id}.pdf";
        var rutaCompleta = Path.Combine(_recibosPath, nombreArchivo);
        
        if (File.Exists(rutaCompleta))
        {
            return $"/recibos/{nombreArchivo}";
        }

        return null;
    }
}

