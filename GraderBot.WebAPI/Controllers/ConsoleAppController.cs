using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraderBot.WebAPI.Controllers
{
    using ProblemTypes.ConsoleApplication;

    [Route("Problems/[Controller]")]
    public abstract class ConsoleAppController<TApp> : ControllerBase
    where TApp : IConsoleApp, new()
    {
        private readonly TApp _consoleApp = new TApp();
        private const string TempDir = @"D:\Users\pdimp\Temp";
        private const string LecturerSourceDir = @"D:\Users\pdimp\GraderBot\Problems";

        [HttpPost("Submit/{problemName}")]
        public async Task<IActionResult> Submit(string problemName, [FromForm] IFormFile problemSolution)
        {
            if (!Directory.Exists(Path.Combine(LecturerSourceDir, problemName)))
                return NotFound();

            var problemTempDir = Directory.CreateDirectory(
                Path.Combine(TempDir, typeof(TApp).Name.Split('`').First() + Path.GetRandomFileName()));

            var sourcePaths = (
                Student: problemTempDir.CreateSubdirectory(Path.Combine("_src_actual_")),
                Lecturer: new DirectoryInfo(Path.Combine(LecturerSourceDir, problemName))
            );

            await using (var stream = problemSolution.OpenReadStream())
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(sourcePaths.Student.FullName);

            var startupClass =
                await System.IO.File.ReadAllTextAsync(
                    sourcePaths.Lecturer.GetFiles("startupClass.txt").First().FullName);

            var input = await Task.WhenAll(sourcePaths.Lecturer
                .GetFiles("input*.txt")
                .Select(f => System.IO.File.ReadAllTextAsync(f.FullName)
                    .ContinueWith(s =>
                        !s.Result.EndsWith("\n")
                            ? s.Result.Contains("\r\n")
                                ? s.Result + "\r\n"
                                : s.Result + '\n'
                            : s.Result)));

            var results = await _consoleApp.TestAsync(problemTempDir, sourcePaths.Student, sourcePaths.Lecturer, startupClass, input, true);

            return Content(Utf8Json.JsonSerializer.ToJsonString(results), MediaTypeNames.Application.Json, Encoding.UTF8);
        }

        [HttpGet("ListAll")]
        [HttpGet("ListAll/{pattern}")]
        public IActionResult ListAll(string pattern = ".*")
        {
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Content(Utf8Json.JsonSerializer
                .ToJsonString(new DirectoryInfo(LecturerSourceDir)
                    .GetDirectories()
                    .Select(d => d.Name)
                    .Where(n => regex.IsMatch(n))), MediaTypeNames.Application.Json, Encoding.UTF8);
        }

        [HttpGet("Hello/{name}")]
        public IActionResult Hello(string name)
        {
            return new JsonResult(new { Text = $"Hello, {name} I said!" });
        }
    }
}