using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Requirements.Models
{
    public class GamesContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLevel> Levels { get; set; }
        public GamesContext() { }
        public GamesContext(DbContextOptions<GamesContext> options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)// og - OrderGame
        {
            //options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GameStoreDb;Trusted_Connection=True;");
            options.UseSqlServer("Data Source=DESKTOP-6NMCAAM\\SQLEXPRESS;User ID = Bread;Password= 5656;Database=GameStoreDb;Integrated Security=True;");
            options.UseLoggerFactory(SqlLogger);
        }
        public static readonly ILoggerFactory SqlLogger = LoggerFactory.Create(builder =>
        {
        });

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderGame>().HasKey(og => new { og.GameId, og.OrderId });
            builder.Entity<OrderGame>().HasOne(og => og.Game).WithMany(game => game.Orders).HasForeignKey(key => key.GameId);
            builder.Entity<OrderGame>().HasOne(og => og.Order).WithMany(order => order.Games).HasForeignKey(key => key.OrderId);

            builder.Entity<GameGenre>().HasKey(gg => new { gg.GameId, gg.GenreId });
            builder.Entity<GameGenre>().HasOne(gg => gg.Game).WithMany(game => game.Genres).HasForeignKey(key => key.GameId);
            builder.Entity<GameGenre>().HasOne(gg => gg.Genre).WithMany(genre => genre.Games).HasForeignKey(key => key.GenreId);

            builder.Entity<Game>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<Publisher>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<Developer>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<Genre>().HasIndex(x => x.Name).IsUnique();

            builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            builder.Entity<User>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<UserLevel>().HasIndex(x => x.Name).IsUnique();
        }
    }
}
