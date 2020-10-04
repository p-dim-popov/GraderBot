using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GraderBot.ProblemTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Utf8Json;

namespace GraderBot.WebAPI.Controllers
{
    using ProblemTypes.ConsoleApplication;
    using Models;

    [Route("Problems/[Controller]")]
    public abstract class ConsoleAppController<TApp> : ControllerBase
    where TApp : IConsoleApp, new()
    {
        private readonly TApp _consoleApp = new TApp();

        private readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        protected ConsoleAppController()
        {
        }

        private const string TempDir = @"D:\Users\pdimp\Temp";
        private const string LecturerSourceDir = @"D:\Users\pdimp\GraderBot\Problems";

        [HttpPost("Submit/{problemName}")]
        public async Task<IActionResult> Submit(string problemName, [FromForm] IFormFile problemSolution)
        {
            if (!Directory.Exists(Path.Combine(LecturerSourceDir, problemName)))
                return NotFound();

            var solutionId = $"{typeof(TApp).Name.Split('`').First()}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}";

            var problemTempDir = Directory.CreateDirectory(Path.Combine(TempDir, solutionId));

            var sourcePaths = (
                Student: problemTempDir.CreateSubdirectory(Path.Combine("_src_actual_")),
                Lecturer: new DirectoryInfo(Path.Combine(LecturerSourceDir, problemName))
            );

            await using (var stream = problemSolution.OpenReadStream())
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(sourcePaths.Student.FullName);

            var config = _deserializer
                .Deserialize<ConsoleAppConfig>(await System.IO.File
                    .ReadAllTextAsync(sourcePaths.Lecturer
                        .GetFiles("config.yaml")
                        .First()
                        .FullName));

            var result = await _consoleApp.TestAsync(problemTempDir, sourcePaths, config.StartupClass, config.Input, true);
            result.Id = solutionId;
            
            return Content(JsonSerializer.ToJsonString(result), MediaTypeNames.Application.Json, Encoding.UTF8);
        }

        [HttpGet("ListAll")]
        [HttpGet("ListAll/{pattern}")]
        public IActionResult ListAll(string pattern = ".*")
        {
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Content(JsonSerializer
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