namespace GraderBot.WebAPI.Controllers
{
    using UnitOfWork;
    using Workers.Compilers;
    using Workers.Runners;
    using ProblemTypes.UnitTestApplication;

    public class JavaUnitTestedAppController : UnitTestedAppController<UnitTestedApp<JavaCompiler, JavaRunner>>
    {
        public JavaUnitTestedAppController(AppUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
    }
}
