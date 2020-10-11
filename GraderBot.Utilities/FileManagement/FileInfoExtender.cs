using System.IO;
using System.Threading.Tasks;

namespace GraderBot.Utilities.FileManagement
{
    public static class FileInfoExtender
    {
        public static Task<byte[]> ReadAllBytesAsync(this FileInfo file)
        {
            return File.ReadAllBytesAsync(file.FullName);
        }

        public static Task<string> ReadAllTextAsync(this FileInfo file)
        {
            return File.ReadAllTextAsync(file.FullName);
        }
    }
}
