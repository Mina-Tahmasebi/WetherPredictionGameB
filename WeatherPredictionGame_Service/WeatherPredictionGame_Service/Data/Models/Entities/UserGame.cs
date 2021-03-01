using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models.Entities
{
    public class UserGame
    {
        public int UserGameId { get; set; }
       
        public int UserId { get; set; }
        public int GameId { get; set; }
        public DateTime Rdate { get; set; }
        public virtual User User { set; get; }
        public virtual Game Game { set; get; }
        public virtual  ICollection<UserGuess> UserGuesses { set; get; }
    }
    public class UserGameConfiguration : IEntityTypeConfiguration<UserGame>
    {
        public void Configure(EntityTypeBuilder<UserGame> builder)
        {
            builder.HasOne(p => p.User).WithMany(c => c.UserGames).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(p => p.Game).WithMany(c => c.UserGames).HasForeignKey(p => p.GameId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
