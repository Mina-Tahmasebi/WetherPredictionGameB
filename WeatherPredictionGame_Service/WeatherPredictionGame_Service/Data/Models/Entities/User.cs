using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public virtual ICollection<UserGame> UserGames { get; set; }
    }
}
