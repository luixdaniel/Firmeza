using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Areas.Admin.ViewModels;

/// <summary>
/// ViewModel para mostrar información de productos (Index, Details)
/// </summary>
public class ProductViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Nombre del Producto")]
    public string Nombre { get; set; } = null!;
    
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = null!;
    
    [Display(Name = "Precio")]
    [DataType(DataType.Currency)]
    public decimal Precio { get; set; }
    
    [Display(Name = "Stock")]
    public int Stock { get; set; }

    [Display(Name = "Categoría")]
    public string CategoriaNombre { get; set; } = null!;
    
    public int CategoriaId { get; set; }
}

