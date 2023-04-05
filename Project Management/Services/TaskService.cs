using Project_Management.Models;
using Project_Management.Repository;

namespace Project_Management.Services
{
    public class TaskService : Service<Item>, ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<Item> GetItemsByProjectId(Guid projectId)
        {
            return _repository.GetAllItemsByProjectId(projectId);
        }
        public void AddTask(Project project, Item item)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (project.Items == null)
            {
                project.Items = new List<Item>();
            }
            item.ProjectId = project.ProjectId;


            _repository.Add(item);
        }
    }
}
