using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiffMatchPatch;
using GraderBot.ProblemTypes.ConsoleApplication;
using GraderBot.Utilities.FileManagement;

namespace GraderBot.ProblemTypes.UnitTestApplication
{
    using Workers.Compilers;
    using Workers.Runners;

    class UnitTestedApp<TCompiler, TRunner> : IUnitTestedApp
        where TCompiler : ICompiler, new()
        where TRunner : IRunner, new()
    {
        private readonly ConsoleApp<TCompiler, TRunner> _app = new ConsoleApp<TCompiler, TRunner>();

        //private readonly diff_match_patch _dmp = new diff_match_patch();
        //private readonly TCompiler _compiler = new TCompiler();
        //private readonly TRunner _runner = new TRunner();

        public async Task<SolutionDto> TestAsync(
            DirectoryInfo tempDir,
            (DirectoryInfo Student, DirectoryInfo Lecturer) sources,
            DirectoryInfo unitTestsPath,
            string className,
            string[] input,
            bool cleanOutputFiles = false)
        {
            var student = (
                Source: sources.Student,
                Merge: tempDir.CreateSubdirectory("_merge_actual_"));

            var lecturer = (
                Source: sources.Lecturer,
                Merge: tempDir.CreateSubdirectory("_merge_expected_"));


            await Task.WhenAll(
                student.Merge.CopyFromAsync(unitTestsPath, student.Source),
                lecturer.Merge.CopyFromAsync(unitTestsPath, lecturer.Source)
                );
            //input is array of methods to run for different tests
            return await _app.TestAsync(tempDir, (student.Merge, lecturer.Merge), className, input, cleanOutputFiles);
        }
    }
}
