using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GraderBot.Utilities.FileManagement;
using Microsoft.AspNetCore.Mvc;
using Utf8Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GraderBot.WebAPI.Controllers
{
    using Models;

    [Route("Problems/[controller]")]
    [ApiController]
    public abstract class AppController<TApp> : ControllerBase
     where TApp : new()
    {
        protected static readonly string TempDirectory = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"Problems\Output");
        protected readonly string LecturerSourceDirectory;

        protected readonly TApp _app = new TApp();

        protected readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        protected AppController()
        {
            this.LecturerSourceDirectory =
                Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                    $@"Problems\{GetType().Name.Replace("Controller", "")}");
        }

        [HttpGet("ListAll")]
        [HttpGet("ListAll/{pattern}")]
        public IActionResult ListAll(string pattern = ".*")
        {
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Content(JsonSerializer
                .ToJsonString(new DirectoryInfo(LecturerSourceDirectory)
                    .GetDirectories()
                    .Select(d => d.Name)
                    .Where(n => regex.IsMatch(n))), MediaTypeNames.Application.Json, Encoding.UTF8);
        }

        [HttpGet("TaskDescription/{problemName}")]
        public async Task<IActionResult> TaskDescription(string problemName)
        {
            return Content(_deserializer
                .Deserialize<AppConfig>(await System.IO.File
                    .ReadAllTextAsync(new DirectoryInfo(LecturerSourceDirectory)
                        .GetDirectories(problemName)
                        .First(d => d.Name == problemName)
                        .GetFiles("config.yaml")
                        .First()
                        .FullName))
                .TaskDescription, MediaTypeNames.Text.Plain, Encoding.UTF8);
        }
    }
}
