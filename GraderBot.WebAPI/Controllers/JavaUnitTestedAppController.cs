
namespace GraderBot.WebAPI.Controllers
{
    using Workers.Compilers;
    using Workers.Runners;
    using ProblemTypes.UnitTestApplication;

    public class JavaUnitTestedAppController : UnitTestedAppController<UnitTestedApp<JavaCompiler, JavaRunner>>
    {
    }
}
