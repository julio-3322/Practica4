using Microsoft.EntityFrameworkCore;
using practica4.Models;

namespace practica4.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Libro> Libro { get; set; }

        public DbSet<Autor> Autor { get; set; }
    }
}
