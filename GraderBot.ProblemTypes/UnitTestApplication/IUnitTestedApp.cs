using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GraderBot.ProblemTypes.UnitTestApplication
{
    public interface IUnitTestedApp
    {
        public Task<SolutionDto> TestAsync(
            DirectoryInfo tempDir,
            (DirectoryInfo Student, DirectoryInfo Lecturer) sources,
            DirectoryInfo unitTestsPath,
            string className,
            string[] input,
            bool cleanOutputFiles = false
        );
    }
}
