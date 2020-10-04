using System;
using System.IO;
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

        //public async Task<SolutionDto> TestMultipleAsync(
        //    DirectoryInfo tempDir,
        //    (DirectoryInfo student, DirectoryInfo lecturer) sources,
        //    string className,
        //    string[] input,
        //    bool cleanOutputFiles = false)
        //{
        //    var student = (
        //        Source: sources.student,
        //        Output: tempDir.CreateSubdirectory("_out_actual_")
        //    );

        //    var lecturer = (
        //        Source: sources.lecturer,
        //        Output: tempDir.CreateSubdirectory("_out_expected_")
        //    );

        //    var solution = new SolutionDto(input.Length);

        //    if (await CompileSourcesAsync(student, lecturer, solution.Outputs) != 0)
        //        return solution;

        //    var outputs = await TestSolutionAsync(input, student, lecturer, className);

        //    Parallel.For(0, input.Length, i =>
        //        // Expected vs Actual
        //        solution.Outputs[i] = new OutputsDto(outputs.Expected[i], outputs.Actual[i]));

        //    if (cleanOutputFiles)
        //    {
        //        student.Output.DeleteRecursive();
        //        lecturer.Output.DeleteRecursive();
        //    }

        //    return solution;
        //}

        public async Task<SolutionDto> TestAsync(
            DirectoryInfo tempDir,
            (DirectoryInfo student, DirectoryInfo lecturer) sources,
            string className,
            string[] input,
            bool cleanOutputFiles = false)
        {
            var student = (
                Source: sources.student,
                Output: tempDir.CreateSubdirectory("_out_actual_")
            );

            var lecturer = (
                Source: sources.lecturer,
                Output: tempDir.CreateSubdirectory("_out_expected_")
            );

            var solution = new SolutionDto(input.Length);

            if (await CompileSourcesAsync(student, lecturer, solution.Outputs) != 0)
                return solution;

            var outputs = await TestSolutionAsync(input, student, lecturer, className);

            Parallel.For(0, input.Length, i =>
                // Expected vs Actual
                solution.Outputs[i] = new OutputsDto(outputs.Expected[i], outputs.Actual[i]));

            if (cleanOutputFiles)
            {
                student.Output.DeleteRecursive();
                lecturer.Output.DeleteRecursive();
            }

            return solution;
        }

        private async Task<int> CompileSourcesAsync((DirectoryInfo Source, DirectoryInfo Output) student, (DirectoryInfo Source, DirectoryInfo Output) lecturer, OutputsDto[] diffsResult)
        {
            var compileResults = await Task.WhenAll(
                _compiler.CompileAsync(student.Source, student.Output).Task,
                _compiler.CompileAsync(lecturer.Source, lecturer.Output).Task
            );

            // If student's solution could not compile
            if (compileResults[0].ExitCode != 0)
            {
                var errorOutput = await compileResults[0].StandardError.ReadToEndAsync();

                Parallel.For(0, diffsResult.Length, i =>
                    diffsResult[i] = new OutputsDto("", errorOutput));
            }

            return compileResults[0].ExitCode;
        }

        private async Task<(string[] Actual, string[] Expected)> TestSolutionAsync(
            string[] input,
            (DirectoryInfo Source, DirectoryInfo Output) student,
            (DirectoryInfo Source, DirectoryInfo Output) lecturer,
            string className)
        {
            var actualOutput = new string[input.Length];
            var expectedOutput = new string[input.Length];

            var tasks = new Task[input.Length * 2];

            for (int i = 0; i < input.Length * 2; i++)
            {
                var index = i;
                tasks[i] = Task.Run(async () =>
                {
                    var isStudentCode = index < input.Length;
                    var codeRef = isStudentCode ? student.Output : lecturer.Output;
                    var realIndex = isStudentCode ? index : index - input.Length;
                    var outputRef = isStudentCode ? actualOutput : expectedOutput;

                    var process = _runner.Run(codeRef, className);

                    await process.StandardInput.WriteAsync(input[realIndex]);
                    process.WaitForExit((int)TimeSpan.FromSeconds(10).TotalMilliseconds);

                    if (process.HasExited)
                    {
                        outputRef[realIndex] = process.ExitCode != 0
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

            return (actualOutput, expectedOutput);
        }
    }
}