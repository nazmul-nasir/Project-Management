using Project_Management.Models;
using Project_Management.Repository;

namespace Project_Management.Services
{
    public class DeveloperService : Service<Developer>, IDeveloperService
    {
        private readonly IDeveloperRepository _developerRepository;
        public DeveloperService(IDeveloperRepository developerRepository) : base(developerRepository)
        {
            _developerRepository = developerRepository;
        }

        public Task<Developer> GetDeveloperByUserId(Guid id)
        {
            return  _developerRepository.GetDeveloperByUserId(id);
        }
    }
}
