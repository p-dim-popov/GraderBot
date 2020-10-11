using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GraderBot.Database.Repositories
{
    using Models;
    using Data;

    public class ResultRepository : Repository<Result>
    {
        public ResultRepository(GraderBotContext dbContext) 
            : base(dbContext) { }

        public override DbSet<Result> DbSet => _dbContext.Results;

        public async Task<string[]> GetResultsOutputByProblemIdAndUserIdAsync(int problemId, int authorId)
        {
            return await DbSet
                .Where(r => r.Solution.ProblemId == problemId && r.Solution.UserId == authorId)
                .Select(r => r.Output)
                .ToArrayAsync();
        }
    }
}
