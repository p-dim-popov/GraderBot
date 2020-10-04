using System.Collections.Generic;

namespace GraderBot.WebAPI.Models
{
    public class ConsoleAppConfig
    {
        public string StartupClass { get; set; }
        public string[] Input { get; set; }
        public string TaskDescription { get; set; }
    }
}
