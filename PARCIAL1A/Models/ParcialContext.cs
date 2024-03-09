using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace PARCIAL1A.Models
{
    public class ParcialContext : DbContext
    {
        public ParcialContext(DbContextOptions<ParcialContext> options) : base(options)
        {

        }
        public DbSet<autor> Autores { get; set; }
        public DbSet<autorlibro> AutorLibro { get; set; }
        public DbSet<post> Posts { get; set; }
        public DbSet<libro> Libros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<autorlibro>().HasNoKey();
        }
    } 
}

