namespace Firmeza.Web.Data.Entities;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public decimal Precio { get; set; }
    public int Stock { get; set; }

    // Relaciones
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
}