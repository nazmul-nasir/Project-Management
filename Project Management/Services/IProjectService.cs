using Project_Management.Models;

namespace Project_Management.Services
{
    public interface IProjectService : IService<Project>
    {
        void AddDeveloperToProject(Project project, Developer developer);
    }
}
