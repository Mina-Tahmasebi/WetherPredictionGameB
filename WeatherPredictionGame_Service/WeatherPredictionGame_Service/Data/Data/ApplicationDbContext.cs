using Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Data
{
    public class ApplicationDbContext :DbContext, IWeatherContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityGame> CityGames { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<UserGuess> UserGuesses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //modelBuilder.Entity<City>().HasData(new City()
            //{
            //    CityId = 1,
            //    Name = "test"
            //});
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity(typeof(City))
        //        .HasOne(typeof(CityGame), "CityGame")
        //        .WithMany()
        //        .HasForeignKey("CityId")
        //        .OnDelete(DeleteBehavior.Restrict);
        //    modelBuilder.Entity(typeof(Game))
        //        .HasOne(typeof(CityGame), "CityGame")
        //        .WithMany()
        //        .HasForeignKey("GameId")
        //        .OnDelete(DeleteBehavior.Restrict);
        //    modelBuilder.Entity(typeof(Game))
        //        .HasOne(typeof(UserGame), "UserGame")
        //        .WithMany()
        //        .HasForeignKey("GameId")
        //        .OnDelete(DeleteBehavior.Restrict);
        //    modelBuilder.Entity(typeof(User))
        //        .HasOne(typeof(UserGame), "UserGame")
        //        .WithMany()
        //        .HasForeignKey("UserId")
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity(typeof(CityGame))
        //        .HasOne(typeof(UserGuess), "UserGuess")
        //        .WithMany()
        //        .HasForeignKey("CityGameId")
        //        .OnDelete(DeleteBehavior.Restrict);
        //    modelBuilder.Entity(typeof(UserGame))
        //        .HasOne(typeof(UserGuess), "UserGuess")
        //        .WithMany()
        //        .HasForeignKey("UserGameId")
        //        .OnDelete(DeleteBehavior.Restrict);
        //}
    }

    public interface IWeatherContext 
    {
        DbSet<User> Users { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<CityGame> CityGames { get; set; }
        DbSet<Game> Games { get; set; }
        DbSet<UserGame> UserGames { get; set; }
        DbSet<UserGuess> UserGuesses { get; set; }
        int SaveChanges();
    }
}
