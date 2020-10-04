using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GraderBot.Workers.Compilers
{
    using Utilities;

    public interface ICompiler
    {
        public abstract string Name { get; }
        
        protected abstract string CollectArgs(DirectoryInfo sourceRoot, DirectoryInfo outputRoot);
        
        public virtual (Process RunningProcess, Task<Process> Task) CompileAsync(DirectoryInfo source,
            DirectoryInfo output)
        {
            var compiler = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Name,
                    Arguments = CollectArgs(source, output),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true,
            };

            Console.WriteLine(compiler.StartInfo.Arguments);
            return (compiler, compiler.StartAsync());
        }
    }
}