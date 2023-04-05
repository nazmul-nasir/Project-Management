using Project_Management.Models;
using Project_Management.Repository;

namespace Project_Management.Services
{
    public class AdminService : Service<Admin>, IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository) : base(adminRepository)
        {
            _adminRepository = adminRepository;
        }
    }
}
