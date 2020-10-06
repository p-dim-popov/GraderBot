using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using GraderBot.ProblemTypes;
using GraderBot.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utf8Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GraderBot.WebAPI.Controllers
{
    using ProblemTypes.UnitTestApplication;

    public class UnitTestedAppController<TApp> : AppController
    where TApp : IUnitTestedApp, new()
    {
        private readonly TApp _app = new TApp();

        private readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        [HttpPost("Submit/{problemName}")]
        public async Task<IActionResult> Submit(string problemName, [FromForm] IFormFile problemSolution)
        {
            if (!Directory.Exists(Path.Combine(LecturerSourceDir, problemName)))
                return NotFound();

            var solutionId = $"{typeof(TApp).Name.Split('`', 2).First()}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}";

            var problemTempDir = Directory.CreateDirectory(Path.Combine(TempDir, solutionId));

            var sourcePaths = (
                Student: problemTempDir.CreateSubdirectory(Path.Combine("_src_actual_")),
                Lecturer: new DirectoryInfo(Path.Combine(LecturerSourceDir, problemName))
            );

            await using (var stream = problemSolution.OpenReadStream())
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(sourcePaths.Student.FullName);

            var config = _deserializer
                .Deserialize<AppConfig>(await System.IO.File
                    .ReadAllTextAsync(sourcePaths.Lecturer
                        .GetFiles("config.yaml")
                        .First()
                        .FullName));

            var outputs = await Task.WhenAll(
                JsonSerializer.DeserializeAsync<string[]>(System.IO.File.OpenRead(sourcePaths.Lecturer.GetFiles("output.json").First().FullName)),
                _app.TestAsync(problemTempDir, sourcePaths.Student, sourcePaths.Lecturer.GetDirectories("tests").First(), config.StartupClass, config.Input, true)
            );

            var solution = new SolutionDto(outputs[0], outputs[1], solutionId);
            return Content(JsonSerializer.ToJsonString(solution), MediaTypeNames.Application.Json, Encoding.UTF8);
        }

    }
}
