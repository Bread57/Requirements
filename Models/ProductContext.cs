using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Requirements.Models
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Key> Keys { get; set; }
        public ProductContext() { }
        public ProductContext(DbContextOptions<ProductContext> options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GameStoreDb;Trusted_Connection=True;");
            options.UseSqlServer("Data Source=DESKTOP-6NMCAAM\\SQLEXPRESS;User ID = Bread;Password= 5656;Database=GameStoreDb;Integrated Security=True;");
        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Product>().HasIndex(x => x.GameId).IsUnique();
        }
    }
}
