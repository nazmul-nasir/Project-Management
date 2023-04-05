using Project_Management.Models;

namespace Project_Management.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly DataContext _dataContext;
        public AdminRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
