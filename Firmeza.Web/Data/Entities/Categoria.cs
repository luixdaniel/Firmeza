namespace Firmeza.Web.Data.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;

    // Navegación inversa hacia productos
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
