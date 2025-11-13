namespace Firmeza.Web.Models.ImportacionMasiva;

public class ImportResultado
{
    public bool Exitoso { get; set; }
    public int TotalFilas { get; set; }
    public int FilasExitosas { get; set; }
    public int FilasConError { get; set; }
    public int ProductosCreados { get; set; }
    public int ProductosActualizados { get; set; }
    public int ClientesCreados { get; set; }
    public int ClientesActualizados { get; set; }
    public int VentasCreadas { get; set; }
    public List<ErrorLog> Errores { get; set; } = new List<ErrorLog>();
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaImportacion { get; set; } = DateTime.UtcNow;
}

public class ErrorLog
{
    public int Fila { get; set; }
    public string Campo { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public string TipoEntidad { get; set; } = string.Empty; // Producto, Cliente, Venta
}

