using System.Linq;
using System.Threading.Tasks;
using GraderBot.Database.Data;
using GraderBot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace GraderBot.Database.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(GraderBotContext dbContext)
            : base(dbContext) { }


        public Task<User> GetUserByNameAsync(string name)
            => DbSet
                .FirstOrDefaultAsync(u => u.Username == name);

        public Task<int> GetUserIdByName(string name)
            => DbSet
                .Where(u => u.Username == name)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();
        
        public override DbSet<User> DbSet => _dbContext.Users;
    }
}
