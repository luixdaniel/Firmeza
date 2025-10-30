
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firmeza.Web.Data.Entities
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaVenta { get; set; } = DateTime.Now;

        [Required]
        public string NumeroFactura { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

        [Required]
        public string Cliente { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal IVA { get; set; }

        public string MetodoPago { get; set; }  // Efectivo, Tarjeta, Transferencia...

        public string Estado { get; set; } = "Completada"; // o Pendiente, Cancelada...

        // 🔗 Relación con DetalleDeVenta
        public ICollection<DetalleDeVenta> Detalles { get; set; }

        // Opcional: Relación con Usuario/Vendedor
        public string Vendedor { get; set; }
    }
}