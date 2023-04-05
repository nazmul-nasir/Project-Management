using Project_Management.Models;

namespace Project_Management.Repository
{
    public interface ITaskRepository : IRepository<Item>
    {
        IEnumerable<Item> GetAllItemsByProjectId(Guid taskId);
    }
}
