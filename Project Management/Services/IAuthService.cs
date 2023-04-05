using Project_Management.Models;

namespace Project_Management.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
    }
}
