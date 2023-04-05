using Project_Management.Models;

namespace Project_Management.Services
{
    public interface IDeveloperService : IService<Developer>
    {
        Task<Developer> GetDeveloperByUserId(Guid id);
    }

}
