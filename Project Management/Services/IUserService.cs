using Project_Management.Models;

namespace Project_Management.Services
{
    public interface IUserService : IService<User>
    {
        //  Task<IActionResult> AuthenticateAsync(string email, string password);
        IEnumerable<User> GetAll();
        Task<User> GetByIdAsync(int id);

    }
}
