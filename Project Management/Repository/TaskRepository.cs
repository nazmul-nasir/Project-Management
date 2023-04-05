using Project_Management.Models;

namespace Project_Management.Repository
{
    public class TaskRepository : Repository<Item>, ITaskRepository
    {
        public TaskRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<Item> GetAllItemsByProjectId(Guid taskId)
        {
            return entities.Where(t => t.ProjectId == taskId);
        }
    }
}
