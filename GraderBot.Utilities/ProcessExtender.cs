using System.Diagnostics;
using System.Threading.Tasks;

namespace GraderBot.Utilities
{
    public static class ProcessExtender
    {
        public static Task<Process> StartAsync(this Process process)
        {
            var tcs = new TaskCompletionSource<Process>();

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(process);
            };

            process.Start();

            return tcs.Task;
        }
    }
}