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
        // public DbSet<Usuario> Usuarios { get; set; }
    }
}