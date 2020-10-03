using System.IO;
using System.Threading.Tasks;
using DiffMatchPatch;

namespace GraderBot.ProblemTypes.ConsoleApplication
{
    using Utilities.FileManagement;
    using Workers.Compilers;
    using Workers.Runners;

    public class ConsoleApp<TCompiler, TRunner> : IConsoleApp
        where TCompiler : ICompiler, new()
        where TRunner : IRunner, new()
    {
        private readonly diff_match_patch _dmp = new diff_match_patch();
        private readonly TCompiler _compiler = new TCompiler();
        private readonly TRunner _runner = new TRunner();

        public ConsoleApp()
        {
        }

        public ICompiler Compiler => _compiler;
        public IRunner Runner => _runner;

        public async Task<DiffsDto[]> TestAsync(DirectoryInfo tempDir,
            DirectoryInfo studentFilesPath,
            DirectoryInfo lecturerFilesPath,
            string className,
            string[] input,
            bool cleanOutputFiles = false)
        {
            var diffsResult = new DiffsDto[input.Length];

            var student = (
                Source: studentFilesPath,
                Output: tempDir.CreateSubdirectory("_out_actual_")
            );

            var lecturer = (
                Source: lecturerFilesPath,
                Output: tempDir.CreateSubdirectory("_out_expected_")
            );

            /////////////////////////////////
            // Compiling solutions

            var compileResults = await Task.WhenAll(
                _compiler.CompileAsync(student.Source, student.Output).Task,
                _compiler.CompileAsync(lecturer.Source, lecturer.Output).Task
            );

            // If student's solution could not compile
            if (compileResults[0].ExitCode != 0)
            {
                var errorOutput = await compileResults[0].StandardError.ReadToEndAsync();

                for (int i = 0; i < diffsResult.Length; i++)
                    diffsResult[i] = new DiffsDto(_dmp.diff_main("", errorOutput), "", errorOutput);

                return diffsResult;
            }

            ////////////////////////////////
            // Testing solutions

            var tasks = new Task[input.Length * 2];

            var actualOutput = new string[input.Length];
            var expectedOutput = new string[input.Length];

            Parallel.For(0, input.Length * 2, i =>
            {
                var index = i;
                tasks[i] = Task.Run(async () =>
                {
                    if (index < input.Length)
                    {
                        var (runningProcess, task) = _runner.RunAsync(student.Output, className);

                        await runningProcess.StandardInput.WriteAsync(input[index]);
                        using var completedTask = await task;

                        actualOutput[index] = completedTask.ExitCode != 0
                            ? await completedTask.StandardError.ReadToEndAsync()
                            : await completedTask.StandardOutput.ReadToEndAsync();
                    }
                    else
                    {
                        index -= input.Length;
                        var (runningProcess, task) = _runner.RunAsync(lecturer.Output, className);

                        await runningProcess.StandardInput.WriteAsync(input[index]);
                        using var completedTask = await task;

                        expectedOutput[index] = completedTask.ExitCode != 0
                            ? await completedTask.StandardError.ReadToEndAsync()
                            : await completedTask.StandardOutput.ReadToEndAsync();
                    }
                });
            });

            await Task.WhenAll(tasks);

            Parallel.For(0, input.Length, i =>
                // Expected vs Actual
                diffsResult[i] = new DiffsDto(_dmp.diff_main(expectedOutput[i], actualOutput[i]), expectedOutput[i], actualOutput[i]));

            if (cleanOutputFiles)
            {
                student.Output.DeleteRecursive();
                lecturer.Output.DeleteRecursive();
            }

            return diffsResult;
        }
    }
}