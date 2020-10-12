using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraderBot.Database.Models
{
    using Enums;

    public class Problem
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public virtual User Author { get; set; }
        public ProblemType Type { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Config { get; set; }
        [Required]
        public byte[] Source { get; set; }

        public virtual ICollection<Solution> Solutions { get; set; }
            = new HashSet<Solution>();
    }
}
