using Project_Management.Models;

namespace Project_Management.Repository
{
    public interface IDeveloperRepository : IRepository<Developer>
    {
        Task<Developer> GetDeveloperByUserId(Guid id);
    }
}
