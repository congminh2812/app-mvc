using AppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AppMVC.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Models.Type> Types { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
