using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraderBot.Database.Models
{
    using Enums;

    public class User
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public UserState State { get; set; }


        public virtual ICollection<Solution> Solutions { get; set; }
            = new HashSet<Solution>();
        public virtual ICollection<Problem> Problems { get; set; }
            = new HashSet<Problem>();
    }
}
