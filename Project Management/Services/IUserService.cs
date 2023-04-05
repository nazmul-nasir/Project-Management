using Project_Management.Models;

namespace Project_Management.Services
{
    public interface IUserService : IService<User>
    {
        IEnumerable<User> GetAll();
        Task<User> GetByIdAsync(int id);

    }
}
