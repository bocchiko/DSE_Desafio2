using Microsoft.EntityFrameworkCore;

namespace LibroAPI.Models
{
    public class LibroContext : DbContext
    {
        public LibroContext(DbContextOptions<LibroContext> options) : base(options) { }

        public DbSet<Libro> Libros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Libro>().HasData(
                new Libro
                {
                    Id = 1,
                    Name = "La Divina Comedia",
                    Autor = "Dante",
                    AnioPublicacion = "2001"
                },
                new Libro
                {
                    Id = 2,
                    Name = "El quijote",
                    Autor = "Miguel de Cervantes",
                    AnioPublicacion = "2001"
                }
                );
        }
    }
}
