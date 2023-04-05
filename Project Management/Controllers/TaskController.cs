using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Management.Models;
using Project_Management.Services;
using System.Security.Claims;

namespace Project_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Developer, Admin")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;
        private readonly IDeveloperService _deverloperService;
        public TaskController(ITaskService taskService, IProjectService projectService, IDeveloperService deverloperService)
        {
            _taskService = taskService;
            _projectService = projectService;
            _deverloperService = deverloperService;
        }

        [HttpGet("get-task/{id}")]
        public ActionResult<Item> Get(Guid id)
        {
            var task = _taskService.GetById(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        [HttpGet("get-task-byproject/{projectId}")]
        public ActionResult<IEnumerable<Item>> GetTasksByProjectId(Guid projectId)
        {
            var tasks = _taskService.GetItemsByProjectId(projectId).ToList();

            if (tasks.Count == 0)
            {
                return NotFound();
            }

            return tasks;
        }

        [HttpPost("add-task")]
        public async Task<IActionResult> AddTask([FromBody] ItemRequestModel taskDto)
        {
            var userId = Guid.Parse(HttpContext.User.Identity.GetUserId());

            var project = _projectService.GetById(taskDto.ProjectId);
            if (project == null)
            {
                return NotFound($"Project with id {taskDto.ProjectId} not found");
            }
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var roleClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var userRole = roleClaim?.Value;

            if (userRole == Role.Developer)
            {
                var developer = await _deverloperService.GetDeveloperByUserId(userId);

                if (developer.ProjectId != taskDto.ProjectId)
                {
                    return BadRequest($"Developer is not assigned to the project with id {taskDto.ProjectId}");
                }
            }

            var item = new Item
            {
                TaskId = Guid.NewGuid(),
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                ProjectId = taskDto.ProjectId
            };

            _taskService.AddTask(project, item);
            return Ok($"Task {item.Title} added successfully to the project {project.Name}");
        }

        [HttpPut("update-task/{id}")]
        public IActionResult UpdateTask(Guid id, [FromBody] ItemUpdateModel taskmodel)
        {
            if (taskmodel == null || id != taskmodel.TaskId)
            {
                return BadRequest();
            }

            var existingTask = _taskService.GetById(id);

            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.TaskId = taskmodel.TaskId;
            existingTask.Description = taskmodel.Description;
            existingTask.IsCompleted = taskmodel.IsCompleted;
            existingTask.ProjectId = taskmodel.ProjectId;

            _taskService.Update(existingTask);

            return NoContent();
        }

        [HttpDelete("delete-task/{id}")]
        public IActionResult DeleteTask(Guid taskId)
        {
            var task = _taskService.GetById(taskId);

            if (task == null)
            {
                return NotFound();
            }

            _taskService.Remove(task);

            return NoContent();
        }
    }
}
