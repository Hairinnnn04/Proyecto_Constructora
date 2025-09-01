using Microsoft.EntityFrameworkCore;

namespace miprimerproyecto.Models
{
    public class ConstructoraDbContext : DbContext
    {
        public ConstructoraDbContext(DbContextOptions<ConstructoraDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<DetalleProyecto> DetalleProyectos { get; set; }
    }
}
