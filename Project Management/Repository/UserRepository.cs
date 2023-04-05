using Microsoft.EntityFrameworkCore;
using Project_Management.Models;

namespace Project_Management.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
    }

}
