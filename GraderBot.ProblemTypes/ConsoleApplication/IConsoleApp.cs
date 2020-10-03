using System.IO;
using System.Threading.Tasks;
using GraderBot.Workers.Compilers;
using GraderBot.Workers.Runners;

namespace GraderBot.ProblemTypes.ConsoleApplication
{
    public interface IConsoleApp
    {
        public ICompiler Compiler { get; }
        public IRunner Runner { get; }

        public Task<DiffsDto[]> TestAsync(DirectoryInfo tempDir,
            DirectoryInfo studentFilesPath,
            DirectoryInfo lecturerFilesPath,
            string className,
            string[] input,
            bool cleanOutputFiles = false);
    }
}
