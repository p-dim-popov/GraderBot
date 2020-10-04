using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DiffMatchPatch;

namespace GraderBot.ProblemTypes
{
    public class SolutionDto
    {
        public SolutionDto(int inputLength)
        {
            this.Outputs = new OutputsDto[inputLength];
        }

        [DataMember(Name = "outputs")]
        public OutputsDto[] Outputs { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}