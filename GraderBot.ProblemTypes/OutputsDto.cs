using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DiffMatchPatch;

namespace GraderBot.ProblemTypes
{
    public class OutputsDto
    {
        //private readonly List<Diff> _diffs;

        public OutputsDto(string expected, string actual)
        {
            Actual = actual;
            Expected = expected;
        }

        //[DataMember(Name = "diffs")]
        //public DiffProxy[] Diffs
        //    => _diffs
        //        .Select(d => new DiffProxy(d))
        //        .ToArray();

        //[DataMember(Name = "hasDiffs")]
        //public bool HasDiffs
        //    => _diffs.Count > 1 && _diffs.FirstOrDefault()?.operation == Operation.EQUAL;

        [DataMember(Name = "expected")]
        public string Expected { get; }

        [DataMember(Name = "actual")]
        public string Actual { get; }
    }
}
