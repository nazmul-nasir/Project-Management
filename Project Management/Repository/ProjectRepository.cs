using Microsoft.EntityFrameworkCore;
using Project_Management.Models;

namespace Project_Management.Repository
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DataContext context) : base(context)
        {
        }

        public void AddDeveloperToProject(Project project, Developer developer)
        {
            developer.ProjectId = project.ProjectId;
            _context.Entry(developer).State = EntityState.Modified;
            _context.SaveChanges();

        }


        public Project GetProjectWithDevelopers(string developerId)
        {
            return _context.Projects
                            .Include(p => p.Developers)
                            .SingleOrDefault(p => p.Developers.Any(d => d.Id == developerId));

        }
    }
}
