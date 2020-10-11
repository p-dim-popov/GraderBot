using System;
using System.IO;
using System.IO.Compression;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utf8Json;

namespace GraderBot.WebAPI.Controllers
{
    using Models;
    using UnitOfWork;
    using ProblemTypes;
    using ProblemTypes.ConsoleApplication;


    public abstract class ConsoleAppController<TApp> : AppController<TApp>
    where TApp : IConsoleApp, new()
    {
        protected ConsoleAppController(AppUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }

        [HttpPost("Submit/{problemName}")]
        public async Task<IActionResult> Submit(string problemName, [FromForm] IFormFile problemSolution)
        {
            //TODO: test
            var problem =
                await _unitOfWork.ProblemRepository
                    .GetProblemByTypeAndNameAsync(AppTypeName, problemName);

            if (problem is null)
                return NotFound();

            var solutionGuid = Guid.NewGuid();

            var solutionDirectory = Directory.CreateDirectory(Path.Combine(TempDirectory, solutionGuid.ToString()));

            var sourcePath = solutionDirectory.CreateSubdirectory("_src_actual_");

            await using (var stream = problemSolution.OpenReadStream())
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(sourcePath.FullName);

            var config = _deserializer
                .Deserialize<AppConfig>(problem.Config);

            var outputs = await Task.WhenAll(
                _unitOfWork.ResultRepository.GetResultsOutputByProblemIdAndUserIdAsync(problem.Id, problem.AuthorId),
                _app.TestAsync(solutionDirectory, sourcePath, config.StartupClass, config.Input, true)
            );

            var solution = new SolutionDto(outputs[0], outputs[1], solutionGuid.ToString().ToUpper());
            var jsonSolution = JsonSerializer.ToJsonString(solution);

            //TODO: add solution to database

            return Content(jsonSolution, MediaTypeNames.Application.Json, Encoding.UTF8);
        }
    }
}