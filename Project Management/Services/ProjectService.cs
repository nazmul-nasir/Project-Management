using Project_Management.Models;
using Project_Management.Repository;

namespace Project_Management.Services
{
    public class ProjectService : Service<Project>, IProjectService
    {
        IProjectRepository _projectRepository;
        public ProjectService(IProjectRepository repository) : base(repository)
        {
            _projectRepository = repository;
        }

        public void AddDeveloperToProject(Project project, Developer developer)
        {
            _projectRepository.AddDeveloperToProject(project, developer);
        }
    }
}
