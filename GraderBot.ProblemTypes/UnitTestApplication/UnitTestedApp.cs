using System.IO;
using System.Threading.Tasks;

namespace GraderBot.ProblemTypes.UnitTestApplication
{
    using Workers.Compilers;
    using Workers.Runners;
    using ConsoleApplication;
    using Utilities.FileManagement;

    public class UnitTestedApp<TCompiler, TRunner> : IUnitTestedApp
        where TCompiler : ICompiler, new()
        where TRunner : IRunner, new()
    {
        private readonly ConsoleApp<TCompiler, TRunner> _app = new ConsoleApp<TCompiler, TRunner>();

        public async Task<string[]> TestAsync(
            DirectoryInfo tempDir,
            DirectoryInfo sourcesStudent,
            DirectoryInfo unitTestsPath,
            string className,
            string[] input,
            bool cleanOutputFiles = false)
        {
            var student = (
                Source: sourcesStudent,
                Merge: tempDir.CreateSubdirectory("_merge_actual_"));


            await student.Merge.CopyFromAsync(unitTestsPath, student.Source);
            //input is array of methods to run for different tests
            return await _app.TestAsync(tempDir, student.Merge, className, input, cleanOutputFiles);
        }
    }
}
