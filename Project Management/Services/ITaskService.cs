using Project_Management.Models;

namespace Project_Management.Services
{
    public interface ITaskService : IService<Item>
    {
        IEnumerable<Item> GetItemsByProjectId(Guid projectId);
        void AddTask(Project project, Item task);
    }
}
