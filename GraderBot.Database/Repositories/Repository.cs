using System;
using Microsoft.EntityFrameworkCore;

namespace GraderBot.Database.Repositories
{
    using Data;
    using Models.Enums;

    public abstract class Repository<T> where T : class
    {
        protected readonly GraderBotContext _dbContext;

        public Repository(GraderBotContext dbContext)
        {
            _dbContext = dbContext;
        }
        public abstract DbSet<T> DbSet { get; }
        protected static ProblemType GetTypeFromString(string type) => Enum.Parse<ProblemType>(type);

    }
}
