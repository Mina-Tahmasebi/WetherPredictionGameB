using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models.Entities
{
    public class UserGuess
    {
        public int UserGuessId { get; set; }
        public int CityGameId { get; set; }
        public int UserGameId { get; set; }
        public byte OrderGuess { get; set; }
        public DateTime Rdate { get; set; }
        public virtual CityGame CityGame { set; get; }
        public virtual UserGame UserGame { set; get; }
    }
    public class UserGuessConfiguration : IEntityTypeConfiguration<UserGuess>
    {
        public void Configure(EntityTypeBuilder<UserGuess> builder)
        {
            builder.HasOne(p => p.CityGame).WithMany(c => c.UserGuesses).HasForeignKey(p => p.CityGameId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(p => p.UserGame).WithMany(c => c.UserGuesses).HasForeignKey(p => p.UserGameId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
