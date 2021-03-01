using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models.Entities
{
    public class CityGame
    {
        public int CityGameId { get; set; }
        public int CityId { get; set; }
        public int GameId { get; set; }
        public byte Order { get; set; }
        public int Temp { get; set; }
        public virtual City City { set; get; }
        public virtual Game Game { set; get; }
        public virtual ICollection<UserGuess> UserGuesses { set; get; }

    }
    public class CityGameConfiguration : IEntityTypeConfiguration<CityGame>
    {
        public void Configure(EntityTypeBuilder<CityGame> builder)
        {
            builder.HasOne(p => p.City).WithMany(c => c.CityGames).HasForeignKey(p => p.CityId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(p => p.Game).WithMany(c => c.CityGames).HasForeignKey(p => p.GameId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
