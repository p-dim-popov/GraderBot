using System.Runtime.Serialization;
using DiffMatchPatch;

namespace GraderBot.ProblemTypes
{
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
