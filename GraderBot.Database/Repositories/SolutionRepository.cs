using System;
using System.Linq;
using System.Threading.Tasks;
using GraderBot.Database.Data;
using GraderBot.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GraderBot.Database.Repositories
{
    public class SolutionRepository : Repository<Solution>
    {
        public SolutionRepository(GraderBotContext dbContext) 
            : base(dbContext) { }

        public override DbSet<Solution> DbSet => _dbContext.Solutions;

        public async Task<EntityEntry<Solution>> AddSolutionAsync(int problemId, int userId, byte[] source, string[] results)
        {
            var solution = new Solution
            {
                ProblemId = problemId,
                UserId = userId,
                Results = results.Select(r => new Result {Output = r}).ToArray(),
                Source = source,
            };

            return await DbSet.AddAsync(solution);
        }
    }
}
