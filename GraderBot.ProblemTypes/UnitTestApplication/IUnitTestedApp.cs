using System.IO;
using System.Threading.Tasks;

namespace GraderBot.ProblemTypes.UnitTestApplication
{
    public interface IUnitTestedApp
    {
        public Task<string[]> TestAsync(
            DirectoryInfo tempDir,
            DirectoryInfo sourcesStudent,
            DirectoryInfo unitTestsPath,
            string className,
            string[] input,
            bool cleanOutputFiles = false
        );
    }
}
