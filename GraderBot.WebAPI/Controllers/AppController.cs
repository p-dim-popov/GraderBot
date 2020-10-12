using System;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utf8Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GraderBot.WebAPI.Controllers
{
    using Database.Models.Enums;
    using UnitOfWork;


    [Route("Problems/[controller]")]
    [ApiController]
    public abstract class AppController<TApp> : ControllerBase
     where TApp : new()
    {
        protected readonly AppUnitOfWork _unitOfWork;

        protected static readonly string TempDirectory =
            Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Temp");
        protected readonly string LecturerSourceDirectory;

        protected readonly string AppTypeName;

        protected readonly TApp _app = new TApp();

        protected readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();


        protected AppController(AppUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this.AppTypeName = GetType().Name.Replace("Controller", "");
            this.LecturerSourceDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), $@"Problems\{AppTypeName}");
        }

        [HttpGet("ListAll")]
        [HttpGet("ListAll/{pattern}")]
        public async Task<IActionResult> ListAll(string pattern)
        {
            return Content(JsonSerializer
                .ToJsonString(await _unitOfWork.ProblemRepository
                    .GetProblemNamesByTypeAndNamePatternAsync(AppTypeName, pattern ?? "")),
                MediaTypeNames.Application.Json, Encoding.UTF8);
        }

        [HttpGet("Description/{problemName}")]
        public async Task<IActionResult> Description(string problemName)
        {
            return Content(await _unitOfWork.ProblemRepository
                    .GetDescriptionByTypeAndNameAsync(AppTypeName, problemName),
                MediaTypeNames.Text.Plain, Encoding.UTF8);
        }

        [HttpPost("Delete/{problemName}")]
        public async Task<IActionResult> Delete(string problemName)
        {
            await _unitOfWork.ProblemRepository.DeleteProblemAsync(AppTypeName, problemName);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
