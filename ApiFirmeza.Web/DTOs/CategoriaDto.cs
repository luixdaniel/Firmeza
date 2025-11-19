namespace ApiFirmeza.Web.DTOs;

public class CategoriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public int CantidadProductos { get; set; }
}

public class CategoriaCreateDto
{
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
}

public class CategoriaUpdateDto
{
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
}

