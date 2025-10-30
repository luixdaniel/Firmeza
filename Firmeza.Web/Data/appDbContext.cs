using Firmeza.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Aquí defines tus tablas (DbSets)
        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;

    }
}