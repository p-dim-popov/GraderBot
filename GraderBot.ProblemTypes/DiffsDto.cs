using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using DiffMatchPatch;

namespace GraderBot.ProblemTypes
{
    public class DiffsDto
    {
        private readonly List<Diff> _diffs;

        public DiffsDto(List<Diff> diffs, string expected, string actual)
        {
            _diffs = diffs;
            Actual = actual;
            Expected = expected;
        }

        [DataMember(Name = "diffs")]
        public DiffProxy[] Diffs
            => _diffs
                .Select(d => new DiffProxy(d))
                .ToArray();
        
        [DataMember(Name = "hasDiffs")]
        public bool HasDiffs 
            => _diffs.Count > 1 && _diffs.FirstOrDefault()?.operation == Operation.EQUAL;

        [DataMember(Name = "expected")]
        public string Expected { get; }

        [DataMember(Name = "actual")]
        public string Actual { get; }

        public void Print()
        {
                var expectedOutput = _diffs
                    .Where(d =>
                        d.operation.Equals(Operation.EQUAL) ||
                        d.operation.Equals(Operation.DELETE)) //Delete will be green for "missing but should be there"
                    .ToList();

                var actualOutput = _diffs
                    .Where(d =>
                        d.operation.Equals(Operation.EQUAL) ||
                        d.operation.Equals(Operation.INSERT)) //Insert will be red for "present but should not be there"
                    .ToList();


                static void PrintDiffs(Diff d)
                {
                    var savedColor = Console.ForegroundColor;
                    Console.ForegroundColor = d.operation switch
                    {
                        Operation.DELETE => ConsoleColor.Green,
                        Operation.INSERT => ConsoleColor.Red,
                        _ => ConsoleColor.Gray
                    };

                    Console.Write(d.text);
                    Console.ForegroundColor = savedColor;
                }

                Console.WriteLine("Expected:");
                expectedOutput.ForEach(PrintDiffs);
                Console.WriteLine();

                Console.WriteLine("Actual:");
                actualOutput.ForEach(PrintDiffs);
                Console.WriteLine();
        }
    }

    public class DiffProxy
    {
        private readonly Diff _diff;

        public DiffProxy(Diff diff)
        {
            _diff = diff;
        }

        [DataMember(Name = "text")]
        public string Text => _diff.text.ToString();

        [DataMember(Name = "operation")]
        public char Operation => _diff.operation.ToString()[0];
    }
}