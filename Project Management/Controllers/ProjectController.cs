using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Management.Models;
using Project_Management.Services;

namespace Project_Management.Controllers
{
    [Authorize(Roles = Role.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("get-project/{id}")]
        public ActionResult<Project> GetProject(Guid id)
        {
            var project = _projectService.GetById(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpGet("get-projects")]
        public ActionResult<IEnumerable<Project>> GetAllProject()
        {
            var projects = _projectService.GetAll().ToList();

            if (projects.Count == 0)
            {
                return NotFound();
            }

            return projects;
        }

        [HttpPost("create-project")]
        public IActionResult CreateProject([FromBody] ProjectRequestModel projectmodel)
        {
            if (projectmodel == null)
            {
                return BadRequest();
            }

            var project = new Project
            {
                Name = projectmodel.Name,
                Description = projectmodel.Description,
                ProjectId = Guid.NewGuid(),
                AdminId = Guid.Parse(HttpContext.User.Identity.GetUserId())

            };

            _projectService.Add(project);

            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, project);
        }

        [HttpPut("update-project/{id}")]
        public IActionResult UpdateProject(Guid id, [FromBody] ProjectUpdateModel project)
        {
            if (project == null || id != project.ProjectId)
            {
                return BadRequest();
            }

            var existingProject = _projectService.GetById(id);

            if (existingProject == null)
            {
                return NotFound();
            }

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;

            _projectService.Update(existingProject);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProject(Guid projectId)
        {
            var project = _projectService.GetById(projectId);

            if (project == null)
            {
                return NotFound();
            }

            _projectService.Remove(project);

            return NoContent();
        }
    }
}
