using Project_Management.Models;

namespace Project_Management.Repository
{
    public interface IProjectRepository : IRepository<Project>
    {
        Project GetProjectWithDevelopers(string id);
        void AddDeveloperToProject(Project project, Developer developer);

    }
}
