using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraderBot.Workers.Compilers
{
    using Utilities.FileManagement;

    public class JavaCompiler : ICompiler
    {
        public string Name { get; } = "javac";

       string ICompiler.CollectArgs(DirectoryInfo sourceRoot, DirectoryInfo outputRoot)
        {
            var args = new List<string>();

            // Add all files needed to compile
            args.AddRange(sourceRoot.GetFilesAndSubFiles("*.java")
                .Select(f => f.FullName));
            
            // Set output dir
            args.Add($"-d {outputRoot.FullName}");
            return string.Join(" ", args);
        }

    }
}