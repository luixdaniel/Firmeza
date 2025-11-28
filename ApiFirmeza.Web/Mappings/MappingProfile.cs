using AutoMapper;
using ApiFirmeza.Web.DTOs;
using Firmeza.Web.Data.Entities;

namespace ApiFirmeza.Web.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeo de Categoria
        CreateMap<Categoria, CategoriaDto>()
            .ForMember(dest => dest.CantidadProductos, 
                opt => opt.MapFrom(src => src.Productos != null ? src.Productos.Count : 0));
        
        CreateMap<CategoriaCreateDto, Categoria>();
        CreateMap<CategoriaUpdateDto, Categoria>();

        // Mapeo de Producto
        CreateMap<Producto, ProductoDto>()
            .ForMember(dest => dest.CategoriaNombre, 
                opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : null));
        
        CreateMap<ProductoCreateDto, Producto>();
        CreateMap<ProductoUpdateDto, Producto>();

        // Mapeo de Cliente
        CreateMap<Cliente, ClienteDto>()
            .ForMember(dest => dest.NombreCompleto, 
                opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"));
        
        CreateMap<ClienteCreateDto, Cliente>()
            .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true));
        
        CreateMap<ClienteUpdateDto, Cliente>();

        // Mapeo de Venta
        CreateMap<Venta, VentaDto>()
            .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.FechaVenta))
            .ForMember(dest => dest.ClienteNombre, 
                opt => opt.MapFrom(src => src.ClienteEntity != null 
                    ? $"{src.ClienteEntity.Nombre} {src.ClienteEntity.Apellido}" 
                    : src.Cliente));
        
        CreateMap<VentaCreateDto, Venta>()
            .ForMember(dest => dest.FechaVenta, opt => opt.Ignore()) // El controlador lo establece
            .ForMember(dest => dest.NumeroFactura, opt => opt.Ignore()) // El controlador lo establece
            .ForMember(dest => dest.Estado, opt => opt.Ignore()) // El controlador lo establece
            .ForMember(dest => dest.Cliente, opt => opt.Ignore()) // El controlador lo establece
            .ForMember(dest => dest.ClienteEntity, opt => opt.Ignore())
            .ForMember(dest => dest.Vendedor, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<VentaUpdateDto, Venta>()
            .ForMember(dest => dest.FechaVenta, opt => opt.MapFrom(src => src.Fecha));

        // Mapeo de DetalleDeVenta
        CreateMap<DetalleDeVenta, DetalleVentaDto>()
            .ForMember(dest => dest.ProductoNombre, 
                opt => opt.MapFrom(src => src.Producto != null ? src.Producto.Nombre : null));
        
        CreateMap<DetalleVentaCreateDto, DetalleDeVenta>()
            .ForMember(dest => dest.Subtotal, 
                opt => opt.MapFrom(src => src.Cantidad * src.PrecioUnitario));
    }
}

