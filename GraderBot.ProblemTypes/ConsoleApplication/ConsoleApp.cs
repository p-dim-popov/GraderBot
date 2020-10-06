using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraderBot.ProblemTypes.ConsoleApplication
{
    using Utilities.FileManagement;
    using Workers.Compilers;
    using Workers.Runners;

    public class ConsoleApp<TCompiler, TRunner> : IConsoleApp
        where TCompiler : ICompiler, new()
        where TRunner : IRunner, new()
    {
        private readonly TCompiler _compiler = new TCompiler();
        private readonly TRunner _runner = new TRunner();

        public ICompiler Compiler => _compiler;
        public IRunner Runner => _runner;

        public async Task<string[]> TestAsync(
            DirectoryInfo tempDir,
            DirectoryInfo studentSources,
            string className,
            string[] input,
            bool cleanOutputFiles = false)
        {
            var student = (
                Source: studentSources,
                Output: tempDir.CreateSubdirectory("_out_actual_")
            );

            // Check if libraries are used and add them
            var libsPath = student.Source.GetDirectories("lib").FirstOrDefault();
            if (libsPath?.Name == "lib")
            {
                await student.Output.CreateSubdirectory("lib").CopyFromAsync(libsPath);
            }

            var compileResults = await _compiler.CompileAsync(student.Source, student.Output).Task;
            if (compileResults.ExitCode != 0)
            {
                var error = await compileResults.StandardError.ReadToEndAsync();
                return Enumerable.Repeat(error, input.Length)
                    .ToArray();
            }

            var output = await TestSolutionAsync(student, className, input);

            if (cleanOutputFiles)
                student.Output.DeleteRecursive();

            return output;
        }

        private async Task<string[]> TestSolutionAsync(
            (DirectoryInfo Source, DirectoryInfo Output) student,
            string className,
            string[] input)
        {
            var actualOutput = new string[input.Length];

            var tasks = new Task[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                var index = i;
                tasks[i] = Task.Run(async () =>
                {
                    var process = _runner.Run(student.Output, className);

                    await process.StandardInput.WriteAsync(input[index]);
                    process.WaitForExit((int)TimeSpan.FromSeconds(10).TotalMilliseconds);

                    if (process.HasExited)
                    {
                        actualOutput[index] = process.ExitCode != 0
                            ? await process.StandardError.ReadToEndAsync()
                            : await process.StandardOutput.ReadToEndAsync();
                    }
                    else
                    {
                        process.Kill(true); //TODO: Check if process is properly killed!!!
                    }

                });
            }

            await Task.WhenAll(tasks);

            return actualOutput;
        }
    }
}