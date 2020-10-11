using System.Threading.Tasks;

namespace GraderBot.WebAPI.UnitOfWork
{
    interface IUnitOfWork
    {
        public Task SaveAsync();
    }
}
