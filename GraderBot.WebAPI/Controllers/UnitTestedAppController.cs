using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utf8Json;

namespace GraderBot.WebAPI.Controllers
{
    using ProblemTypes;
    using ProblemTypes.UnitTestApplication;
    using Utilities.FileManagement;
    using Models;


    public class UnitTestedAppController<TApp> : AppController<TApp>
        where TApp : IUnitTestedApp, new()

    {
        [HttpPost("Submit/{problemName}")]
        public async Task<IActionResult> Submit(string problemName, [FromForm] IFormFile problemSolution)
        {
            if (!Directory.Exists(Path.Combine(LecturerSourceDirectory, problemName)))
                return NotFound();

            var solutionId = $"{typeof(TApp).Name.Split('`', 2).First()}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}";

            var solutionDirectory = Directory.CreateDirectory(Path.Combine(TempDirectory, solutionId));

            var sourcePaths = (
                Student: solutionDirectory.CreateSubdirectory(Path.Combine("_src_actual_")),
                Lecturer: new DirectoryInfo(Path.Combine(LecturerSourceDirectory, problemName))
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
                _app.TestAsync(solutionDirectory, sourcePaths.Student, sourcePaths.Lecturer.GetDirectories("tests").First(), config.StartupClass, config.Input, true)
            );

            var solution = new SolutionDto(outputs[0], outputs[1], solutionId);
            var jsonSolution = JsonSerializer.ToJsonString(solution);


            _ = solutionDirectory.CreateTextFile("output.json", jsonSolution);

            return Content(jsonSolution, MediaTypeNames.Application.Json, Encoding.UTF8);
        }

    }
}
