using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraderBot.Utilities.FileManagement
{
    public static class DirectoryInfoExtender
    {
        public static void DeleteRecursive(this DirectoryInfo dir)
        {
            foreach (var fileInfo in dir.GetFiles())
                fileInfo.Delete();

            foreach (var directoryInfo in dir.GetDirectories())
                directoryInfo.DeleteRecursive();

            dir.Delete();
        }

        public static IEnumerable<FileInfo> GetFilesAndSubFiles(this DirectoryInfo dir, string fileSearchPattern = "*")
        {
            var pending = new Queue<FileSystemInfo>();
            pending.Enqueue(dir);
            while (pending.Count > 0)
            {
                dir = (DirectoryInfo)pending.Dequeue();
                FileSystemInfo[] tmp;
                try
                {
                    tmp = dir.GetFiles(fileSearchPattern);

                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }

                foreach (var file in tmp)
                {
                    yield return (FileInfo)file;
                }

                tmp = dir.GetDirectories();

                foreach (var directory in tmp)
                {
                    pending.Enqueue(directory);
                }
            }
        }

        public static Task CopyFromAsync(this DirectoryInfo dir, params DirectoryInfo[] sources)
        {
            return Task.Run(() =>
            {
                foreach (var source in sources)
                {
                    if (!source.Exists)
                        throw new DirectoryNotFoundException($"Directory '{source.FullName}' not found");

                    var directoriesToProcess = new Queue<(string sourcePath, string destinationPath)>();
                    directoriesToProcess.Enqueue((sourcePath: source.FullName, destinationPath: dir.FullName));
                    while (directoriesToProcess.Any())
                    {
                        (string sourcePath, string destinationPath) = directoriesToProcess.Dequeue();

                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);

                        var sourceDirectoryInfo = new DirectoryInfo(sourcePath);
                        foreach (FileInfo sourceFileInfo in sourceDirectoryInfo.EnumerateFiles())
                            sourceFileInfo.CopyTo(Path.Combine(destinationPath, sourceFileInfo.Name), true);

                        foreach (DirectoryInfo sourceSubDirectoryInfo in sourceDirectoryInfo.EnumerateDirectories())
                            directoriesToProcess.Enqueue((
                                sourcePath: sourceSubDirectoryInfo.FullName,
                                destinationPath: Path.Combine(destinationPath, sourceSubDirectoryInfo.Name)));
                    }
                }
            });
        }

        public static async Task CreateTextFile(this DirectoryInfo dir, string filename, string contents)
        {
            await using var file = File.CreateText(Path.Combine(dir.FullName, filename));
            await file.WriteAsync(contents);
        }
    }
}