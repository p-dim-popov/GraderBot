using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace GraderBot.Workers.Runners
{
    using Utilities.FileManagement;


    public class JavaRunner : IRunner
    {
        public string Name { get; } = "java";

       string IRunner.CollectArgs(DirectoryInfo root, string className)
        {
            var args = new List<string>();

            // Check if libraries are used and add them
            var libsPath = root.GetDirectories("lib").FirstOrDefault();
            if (libsPath?.Name == "lib")
            {
                var separator = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ';' : ':';
                args.Add($@"-cp ""{root.FullName}""{separator}" + string.Join(separator, root.GetFilesAndSubFiles("*.jar")
                    .Select(f => $@"""{f.FullName}""")));
            }

            args.Add(className);

            return string.Join(" ", args);
        }
    }
}