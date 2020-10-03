using System;
using System.IO;
using System.Threading.Tasks;
using DiffMatchPatch;
using GraderBot.ProblemTypes.ConsoleApplication;
using GraderBot.Utilities.FileManagement;

namespace GraderBot.ProblemTypes.UnitTestApplication
{
    using Workers.Compilers;
    using Workers.Runners;

    class UnitTestApp<TCompiler, TRunner> : IUnitTestApp
        where TCompiler : ICompiler, new()
        where TRunner : IRunner, new()
    {
        private readonly diff_match_patch _dmp = new diff_match_patch();
        private readonly TCompiler _compiler = new TCompiler();
        private readonly TRunner _runner = new TRunner();

        public async Task<DiffsDto[]> TestAsync(
            string tempDir,
            DirectoryInfo studentFilesPath,
            DirectoryInfo lecturerFilesPath,
            DirectoryInfo[] unitTestsPaths,
            bool cleanOutputFiles = false)
        {
            var diffsResult = new DiffsDto[unitTestsPaths.Length];

            var student = (
                Source: studentFilesPath,
                Output: Directory
                    .CreateDirectory(Path.Combine(tempDir, $"{_compiler.Name}_actual_" + Path.GetRandomFileName()))
            );

            var lecturer = (
                Source: lecturerFilesPath,
                Output: Directory
                    .CreateDirectory(Path.Combine(tempDir, $"{_compiler.Name}_expected_" + Path.GetRandomFileName()))
            );

            foreach (var unitTestsPath in unitTestsPaths)
            {
                await Task.WhenAll(
                student.Output.CopyFromAsync(unitTestsPath, student.Source),
                lecturer.Output.CopyFromAsync(unitTestsPath, lecturer.Source)
                );


            }

            return diffsResult;
        }
    }
}
