using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Utf8Json;

namespace GraderBot.WebAPI.Controllers
{
    [Route("Problems/[controller]")]
    [ApiController]
    public abstract class AppController : ControllerBase
    {
        protected const string TempDir = @"D:\Users\pdimp\Temp";
        protected readonly string LecturerSourceDir;

        protected AppController()
        {
            this.LecturerSourceDir =
                $@"D:\Users\pdimp\GraderBot\Problems\{GetType().Name.Replace("Controller", "")}";
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
