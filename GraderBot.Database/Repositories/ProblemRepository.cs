using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GraderBot.Database.Repositories
{
    using Data;
    using Models;

    public class ProblemRepository : Repository<Problem>
    {
        public ProblemRepository(GraderBotContext dbContext)
            : base(dbContext) { }

        public override DbSet<Problem> DbSet => _dbContext.Problems;

        public async Task<IEnumerable<Problem>> GetProblemsAsync()
            => await DbSet.ToListAsync();

        public async Task<Problem> GetProblemByTypeAndNameAsync(string type, string name)
            => await DbSet.FirstOrDefaultAsync(p => p.Type == GetTypeFromString(type) && p.Name == name);

        public async Task<int> GetProblemIdByTypeAndNameAsync(string type, string name)
            => await DbSet
                .Where(p => p.Type == GetTypeFromString(type) && p.Name == name)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

        public async ValueTask<EntityEntry<Problem>> AddProblemAsync(
            string type,
            string name,
            string description,
            byte[] problemArchive,
            string config,
            string[] output,
            int userId)
        {
            var problem = new Problem
            {
                AuthorId = userId,
                Config = config,
                Description = description,
                Name = name,
                Type = GetTypeFromString(type),
                Source = problemArchive,
            };
            problem.Solutions.Add(new Solution
            {
                UserId = userId,
                Results = output
                    .Select(o => new Result { Output = o })
                    .ToArray(),
                Source = problemArchive,
            });

            return await DbSet.AddAsync(problem);
        }

        public async Task<IEnumerable<string>> GetProblemNamesByTypeAndNamePatternAsync(string type, string pattern)
        {
            return await DbSet
                .Where(p => p.Type == GetTypeFromString(type) && p.Name.ToLower().Contains(pattern.ToLower()))
                .Select(p => p.Name)
                .ToListAsync();
        }

        public Task SaveAsync()
            => _dbContext.SaveChangesAsync();

        public async Task<string> GetDescriptionByTypeAndNameAsync(string type, string problemName)
        {
            return await DbSet
                .Where(p => p.Type == GetTypeFromString(type) && p.Name.ToLower() == problemName.ToLower())
                .Select(p => p.Description)
                .FirstAsync();
        }

        public async Task DeleteProblemAsync(string type, string problemName)
        {
            var problem = await DbSet
                .Include(p => p.Solutions)
                .ThenInclude(s => s.Results)
                .Where(p => p.Type == GetTypeFromString(type) && p.Name == problemName)
                .FirstOrDefaultAsync();

            _dbContext.Results.RemoveRange(problem.Solutions.SelectMany(s => s.Results));
            _dbContext.Solutions.RemoveRange(problem.Solutions);
            DbSet.Remove(problem);
        }
    }
}
