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
    using Models;
    using UnitOfWork;

    public class UnitTestedAppController<TApp> : AppController<TApp>
        where TApp : IUnitTestedApp, new()

    {
        public UnitTestedAppController(AppUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        [HttpPost("Submit/{problemName}")]
        public async Task<IActionResult> Submit(string problemName, [FromForm] IFormFile problemSolution)
        {
            var problem =
                await _unitOfWork.ProblemRepository
                    .GetProblemByTypeAndNameAsync(AppTypeName, problemName);

            if (problem is null)
                return NotFound();

            var solutionTempGuid = Guid.NewGuid();

            var solutionDirectory = Directory.CreateDirectory(Path.Combine(TempDirectory, solutionTempGuid.ToString()));

            var sourcePaths = (
                Student: solutionDirectory.CreateSubdirectory("_src_actual_"),
                Lecturer: solutionDirectory.CreateSubdirectory("_src__tests_and_expected_")
            );

            await using (var stream = new MemoryStream(problem.Source))
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(sourcePaths.Lecturer.FullName);

            await using (var stream = problemSolution.OpenReadStream())
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(sourcePaths.Student.FullName);

            var config = _deserializer
                .Deserialize<AppConfig>(problem.Config);

            var outputs = await Task.WhenAll(
                _unitOfWork.ResultRepository.GetResultsOutputByProblemIdAndUserIdAsync(problem.Id, problem.AuthorId),
                _app.TestAsync(solutionDirectory, sourcePaths.Student, sourcePaths.Lecturer.GetDirectories("tests").First(), config.StartupClass, config.Input, true)
            );

            //TODO: add solution to database, and send real guid

            var solution = new SolutionDto(outputs[0], outputs[1], solutionTempGuid.ToString().ToUpper());
            var jsonSolution = JsonSerializer.ToJsonString(solution);

            return Content(jsonSolution, MediaTypeNames.Application.Json, Encoding.UTF8);
        }

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromForm] string name, [FromForm] IFormFile problemFiles)
        {
            var tempId = $"{AppTypeName}_{Guid.NewGuid()}";
            var tempDirectory = Directory.CreateDirectory(Path.Combine(TempDirectory, tempId));

            var userId = await _unitOfWork.UserRepository.GetUserIdByName("pdimp");

            byte[] problemArchive;
            await using (var stream = problemFiles.OpenReadStream())
            {
                await using var memStream = new MemoryStream();
                await stream.CopyToAsync(memStream);
                problemArchive = memStream.ToArray();

                using var archive = new ZipArchive(memStream, ZipArchiveMode.Read, true);
                archive.ExtractToDirectory(tempDirectory.FullName);
            }

            var configFileString = await System.IO.File
                .ReadAllTextAsync(tempDirectory
                    .GetFiles("config.yaml")
                    .First()
                    .FullName);

            var config = _deserializer
                .Deserialize<AppConfig>(configFileString);

            var solutionDirectory = tempDirectory.GetDirectories("expected").First();
            var testsDirectory = tempDirectory.GetDirectories("tests").First();

            var output = await _app.TestAsync(tempDirectory,
                solutionDirectory,
                testsDirectory,
                config.StartupClass,
                config.Input,
                true);

            await _unitOfWork.ProblemRepository
                .AddProblemAsync(
                    AppTypeName,
                    name,
                    config.TaskDescription,
                    problemArchive,
                    configFileString,
                    output,
                    userId);

            await _unitOfWork.SaveAsync();

            return Ok();
        }
    }
}
