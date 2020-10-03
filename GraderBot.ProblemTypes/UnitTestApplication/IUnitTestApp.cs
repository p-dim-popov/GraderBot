using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GraderBot.ProblemTypes.UnitTestApplication
{
    public interface IUnitTestApp
    {
        public Task<DiffsDto[]> TestAsync(
            string tempDir,
            DirectoryInfo studentFilesPath,
            DirectoryInfo lecturerFilesPath,
            DirectoryInfo[] unitTestsPath,
            bool cleanOutputFiles = false
        );
    }
}
