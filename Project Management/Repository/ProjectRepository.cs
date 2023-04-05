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
            project.Developers ??= new List<Developer>();
            var existingDeveloper = project.Developers.SingleOrDefault(d => d.Id == developer.Id && d.ProjectId == project.ProjectId);
            if (existingDeveloper == null)
            {
                project.Developers.Add(developer);
            }
            else
            {
                existingDeveloper.Role = developer.Role;
                _context.Entry(existingDeveloper).State = EntityState.Modified;
            }
        }


        public Project GetProjectWithDevelopers(string userId)
        {
            return _context.Projects
         .Include(p => p.Developers)
         .SingleOrDefault(p => p.Developers.Any(d => d.Id == userId));

        }
    }
}
