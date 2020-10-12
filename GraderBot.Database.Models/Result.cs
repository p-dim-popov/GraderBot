using System;

namespace GraderBot.Database.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string Output { get; set; }
        public Guid SolutionId { get; set; }
        public virtual Solution Solution { get; set; }
    }
}
