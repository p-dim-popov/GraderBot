using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace GraderBot.Workers.Compilers
{
    using Utilities.FileManagement;

    public class JavaCompiler : ICompiler
    {
        public string Name { get; } = "javac";

        string ICompiler.CollectArgs(DirectoryInfo sourceRoot, DirectoryInfo outputRoot)
        {
            var args = new List<string>();

            // Check if libraries are used and add them
            var libsPath = sourceRoot.GetDirectories("lib").FirstOrDefault();
            if (libsPath?.Name == "lib")
            {
                var separator = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ';' : ':';
                args.Add(@$"-cp ""{sourceRoot.FullName}""{separator}" + string.Join(separator, sourceRoot.GetFilesAndSubFiles("*.jar")
                    .Select(f => $@"""{f.FullName}""")));
            }

            // Add all files needed to compile
            args.AddRange(sourceRoot.GetFilesAndSubFiles("*.java")
                .Select(f => $@"""{f.FullName}"""));

            // Set output dir
            args.Add(@$"-d ""{outputRoot.FullName}""");
            return string.Join(" ", args);
        }

    }
}