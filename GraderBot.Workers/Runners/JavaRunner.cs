using System.IO;

namespace GraderBot.Workers.Runners
{

    public class JavaRunner : IRunner
    {
        public string Name { get; } = "java";

       string IRunner.CollectArgs(DirectoryInfo root, string className)
        {
            return $"-cp {root.FullName} {className}";
        }
    }
}