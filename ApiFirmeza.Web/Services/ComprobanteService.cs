using Firmeza.Web.Data.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ApiFirmeza.Web.Services;

public interface IComprobanteService
{
    byte[] GenerarComprobantePdf(Venta venta);
}

public class ComprobanteService : IComprobanteService
{
    public byte[] GenerarComprobantePdf(Venta venta)
    {
        using (var memoryStream = new MemoryStream())
        {
            // Crear documento PDF
            var document = new Document(PageSize.A4, 50, 50, 50, 50);
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // Fuentes
            var tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.DARK_GRAY);
            var subtituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK);
            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK);
            var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.GRAY);

            // Encabezado
            var headerTable = new PdfPTable(2) { WidthPercentage = 100 };
            headerTable.SetWidths(new float[] { 60, 40 });

            // Logo y nombre de empresa
            var logoCell = new PdfPCell(new Phrase("FIRMEZA", tituloFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingBottom = 10
            };
            headerTable.AddCell(logoCell);

            // Información de factura
            var facturaInfo = new Paragraph();
            facturaInfo.Add(new Chunk("COMPROBANTE DE COMPRA\n", subtituloFont));
            facturaInfo.Add(new Chunk($"Factura: {venta.NumeroFactura}\n", normalFont));
            facturaInfo.Add(new Chunk($"Fecha: {venta.FechaVenta:dd/MM/yyyy HH:mm}\n", normalFont));
            var facturaCell = new PdfPCell(facturaInfo)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            headerTable.AddCell(facturaCell);

            document.Add(headerTable);
            document.Add(new Paragraph(" "));

            // Línea separadora
            var linea = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_CENTER, -2)));
            document.Add(linea);
            document.Add(new Paragraph(" "));

            // Información del cliente
            var clienteTable = new PdfPTable(2) { WidthPercentage = 100 };
            clienteTable.SetWidths(new float[] { 20, 80 });

            AddCellToTable(clienteTable, "Cliente:", boldFont, Element.ALIGN_LEFT);
            AddCellToTable(clienteTable, venta.Cliente ?? "N/A", normalFont, Element.ALIGN_LEFT);

            AddCellToTable(clienteTable, "ID Cliente:", boldFont, Element.ALIGN_LEFT);
            AddCellToTable(clienteTable, venta.ClienteId?.ToString() ?? "N/A", normalFont, Element.ALIGN_LEFT);

            AddCellToTable(clienteTable, "Método de Pago:", boldFont, Element.ALIGN_LEFT);
            AddCellToTable(clienteTable, venta.MetodoPago ?? "Efectivo", normalFont, Element.ALIGN_LEFT);

            AddCellToTable(clienteTable, "Estado:", boldFont, Element.ALIGN_LEFT);
            AddCellToTable(clienteTable, venta.Estado ?? "Completada", normalFont, Element.ALIGN_LEFT);

            document.Add(clienteTable);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_CENTER, -2))));
            document.Add(new Paragraph(" "));

            // Tabla de productos
            document.Add(new Paragraph("DETALLE DE PRODUCTOS", subtituloFont));
            document.Add(new Paragraph(" "));

            var productosTable = new PdfPTable(5) { WidthPercentage = 100 };
            productosTable.SetWidths(new float[] { 10, 40, 15, 15, 20 });

            // Encabezados de tabla
            AddHeaderCell(productosTable, "ID", boldFont);
            AddHeaderCell(productosTable, "Producto", boldFont);
            AddHeaderCell(productosTable, "Cantidad", boldFont);
            AddHeaderCell(productosTable, "Precio Unit.", boldFont);
            AddHeaderCell(productosTable, "Subtotal", boldFont);

            // Detalles de productos
            if (venta.Detalles != null && venta.Detalles.Any())
            {
                foreach (var detalle in venta.Detalles)
                {
                    AddDataCell(productosTable, detalle.ProductoId.ToString(), normalFont, Element.ALIGN_CENTER);
                    AddDataCell(productosTable, detalle.Producto?.Nombre ?? "N/A", normalFont, Element.ALIGN_LEFT);
                    AddDataCell(productosTable, detalle.Cantidad.ToString(), normalFont, Element.ALIGN_CENTER);
                    AddDataCell(productosTable, $"${detalle.PrecioUnitario:N2}", normalFont, Element.ALIGN_RIGHT);
                    AddDataCell(productosTable, $"${detalle.Subtotal:N2}", normalFont, Element.ALIGN_RIGHT);
                }
            }

            document.Add(productosTable);
            document.Add(new Paragraph(" "));

            // Totales
            var totalesTable = new PdfPTable(2) { WidthPercentage = 100 };
            totalesTable.SetWidths(new float[] { 70, 30 });

            AddCellToTable(totalesTable, "Subtotal:", boldFont, Element.ALIGN_RIGHT);
            AddCellToTable(totalesTable, $"${venta.Subtotal:N2}", normalFont, Element.ALIGN_RIGHT);

            AddCellToTable(totalesTable, "IVA (16%):", boldFont, Element.ALIGN_RIGHT);
            AddCellToTable(totalesTable, $"${venta.IVA:N2}", normalFont, Element.ALIGN_RIGHT);

            // Total destacado
            var totalLabelCell = new PdfPCell(new Phrase("TOTAL:", 
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE)))
            {
                BackgroundColor = new BaseColor(102, 126, 234),
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 8,
                Border = Rectangle.NO_BORDER
            };
            totalesTable.AddCell(totalLabelCell);

            var totalValueCell = new PdfPCell(new Phrase($"${venta.Total:N2}", 
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE)))
            {
                BackgroundColor = new BaseColor(102, 126, 234),
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 8,
                Border = Rectangle.NO_BORDER
            };
            totalesTable.AddCell(totalValueCell);

            document.Add(totalesTable);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            // Pie de página
            document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_CENTER, -2))));
            document.Add(new Paragraph(" "));
            
            var footer = new Paragraph(
                "Gracias por su compra. Este documento es un comprobante válido de su transacción.\n" +
                $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm:ss}\n" +
                "Para cualquier consulta, contáctenos.",
                smallFont
            );
            footer.Alignment = Element.ALIGN_CENTER;
            document.Add(footer);

            document.Close();
            writer.Close();

            return memoryStream.ToArray();
        }
    }

    private void AddCellToTable(PdfPTable table, string text, Font font, int alignment)
    {
        var cell = new PdfPCell(new Phrase(text, font))
        {
            Border = Rectangle.NO_BORDER,
            HorizontalAlignment = alignment,
            PaddingBottom = 5,
            PaddingTop = 5
        };
        table.AddCell(cell);
    }

    private void AddHeaderCell(PdfPTable table, string text, Font font)
    {
        var cell = new PdfPCell(new Phrase(text, font))
        {
            BackgroundColor = new BaseColor(240, 240, 240),
            HorizontalAlignment = Element.ALIGN_CENTER,
            Padding = 8,
            Border = Rectangle.BOTTOM_BORDER,
            BorderColor = BaseColor.GRAY
        };
        table.AddCell(cell);
    }

    private void AddDataCell(PdfPTable table, string text, Font font, int alignment)
    {
        var cell = new PdfPCell(new Phrase(text, font))
        {
            HorizontalAlignment = alignment,
            Padding = 5,
            Border = Rectangle.NO_BORDER,
            PaddingBottom = 3
        };
        table.AddCell(cell);
    }
}

