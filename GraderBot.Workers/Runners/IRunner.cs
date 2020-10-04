using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GraderBot.Workers.Runners
{
    using Utilities;

    public interface IRunner
    {
        public abstract string Name { get; }
        protected abstract string CollectArgs(DirectoryInfo root, string className);
        public virtual Process Run(DirectoryInfo root, string className)
        {
            var runner = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Name,
                    Arguments = CollectArgs(root, className),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                },
                EnableRaisingEvents = true,
            };
            runner.Start();
            return runner;
        }
    }
}