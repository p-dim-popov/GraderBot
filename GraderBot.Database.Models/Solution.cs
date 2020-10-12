using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraderBot.Database.Models
{
    public class Solution
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ProblemId { get; set; }
        public virtual Problem Problem { get; set; }
        [Required]
        public byte[] Source { get; set; }

        public virtual ICollection<Result> Results { get; set; }
            = new HashSet<Result>();
    }
}
