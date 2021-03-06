﻿using System.IO;
using System.Threading.Tasks;

namespace GraderBot.ProblemTypes.ConsoleApplication
{
    using Workers.Compilers;
    using Workers.Runners;

    public interface IConsoleApp
    {
        public ICompiler Compiler { get; }
        public IRunner Runner { get; }

        public Task<string[]> TestAsync(
            DirectoryInfo tempDir,
            DirectoryInfo studentSources,
            string className,
            string[] input,
            bool cleanOutputFiles = false);
    }
}
