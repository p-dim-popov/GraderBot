using System.Threading.Tasks;

namespace GraderBot.WebAPI.UnitOfWork
{
    using Database.Data;
    using Database.Repositories;

    public class AppUnitOfWork : IUnitOfWork
    {
        protected readonly ProblemRepository _problemRepository;
        protected readonly SolutionRepository _solutionRepository;
        protected readonly UserRepository _userRepository;
        protected readonly ResultRepository _resultRepository;
        private readonly GraderBotContext _dbContext;

        public AppUnitOfWork(GraderBotContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ProblemRepository ProblemRepository => _problemRepository ?? new ProblemRepository(_dbContext);
        public SolutionRepository SolutionRepository => _solutionRepository ?? new SolutionRepository(_dbContext);
        public UserRepository UserRepository => _userRepository ?? new UserRepository(_dbContext);
        public ResultRepository ResultRepository => _resultRepository ?? new ResultRepository(_dbContext);

        public Task SaveAsync()
            => _dbContext.SaveChangesAsync();
    }
}
