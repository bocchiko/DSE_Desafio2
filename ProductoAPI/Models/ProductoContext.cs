using Microsoft.EntityFrameworkCore;

namespace ProductoAPI.Models
{
    public class ProductoContext : DbContext
    {
        public ProductoContext(DbContextOptions<ProductoContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    Id = 1,
                    Nombre = "Laptop",
                    Categoria = "Electrica",
                    Descripcion = "PC XD"
                },
                new Producto
                {
                    Id = 2,
                    Nombre = "Smartphone",
                    Categoria = "Electrica",
                    Descripcion = "Celular mi bro"
                }
                );
        }
    }
}
