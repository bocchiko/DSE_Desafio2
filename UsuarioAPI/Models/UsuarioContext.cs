using Microsoft.EntityFrameworkCore;

namespace UsuarioAPI.Models
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Datos semilla (Seed data) para usuarios
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    Age = 30,
                    Password = "password"
                },
                new Usuario
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Email = "jane@example.com",
                    Age = 25,
                    Password = "password"
                }
            );
        }
    }
}
