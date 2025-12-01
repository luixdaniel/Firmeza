using Firmeza.Web.Data.Entities;
using Firmeza.Web.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Aquí defines tus tablas (DbSets)
        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Venta> Ventas { get; set; } = null!;
        public DbSet<DetalleDeVenta> DetallesDeVenta { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Venta>().ToTable("Ventas");
            modelBuilder.Entity<DetalleDeVenta>().ToTable("DetallesDeVenta");

            // Configurar relación Venta -> DetalleDeVenta con cascada
            modelBuilder.Entity<Venta>()
                .HasMany(v => v.Detalles)
                .WithOne(d => d.Venta)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar relación Producto -> DetalleDeVenta
            modelBuilder.Entity<DetalleDeVenta>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relación Cliente -> Venta (opcional)
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Ventas)
                .WithOne(v => v.ClienteEntity)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
