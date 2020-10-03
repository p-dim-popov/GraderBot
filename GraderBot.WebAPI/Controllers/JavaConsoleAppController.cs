namespace GraderBot.WebAPI.Controllers
{
    using Workers.Compilers;
    using Workers.Runners;

    using ProblemTypes.ConsoleApplication;

    public class JavaConsoleAppController
        : ConsoleAppController<ConsoleApp<JavaCompiler, JavaRunner>>
    {
    }
}