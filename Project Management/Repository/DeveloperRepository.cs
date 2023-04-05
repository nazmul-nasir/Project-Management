using Microsoft.EntityFrameworkCore;
using Project_Management.Models;

namespace Project_Management.Repository
{
    public class DeveloperRepository : Repository<Developer>, IDeveloperRepository
    {
        private readonly DataContext _dataContext;
        public DeveloperRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Developer> GetDeveloperByUserId(Guid id)
        {
            return  await _dataContext.Developers
        .Include(d => d.Project)
        .FirstOrDefaultAsync(d => Guid.Parse(d.Id) == id);
        }
    }
}
