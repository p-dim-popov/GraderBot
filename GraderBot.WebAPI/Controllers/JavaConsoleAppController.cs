namespace GraderBot.WebAPI.Controllers
{
    using UnitOfWork;
    using Workers.Compilers;
    using Workers.Runners;

    using ProblemTypes.ConsoleApplication;

    public class JavaConsoleAppController
        : ConsoleAppController<ConsoleApp<JavaCompiler, JavaRunner>>
    {
        public JavaConsoleAppController(AppUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}