using System.Linq;
using System.Runtime.Serialization;

namespace GraderBot.ProblemTypes
{
    public class SolutionDto
    {
        public SolutionDto(string[] expected, string[] actual, string id)
        {
            this.Outputs = Enumerable.Range(0, expected.Length)
                .Select(i => new OutputsDto(expected[i]?.Trim(), actual[i]?.Trim()))
                .ToArray();
            this.Id = id;
        }

        [DataMember(Name = "outputs")]
        public OutputsDto[] Outputs { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}