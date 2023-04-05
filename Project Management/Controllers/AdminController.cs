using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Management.Services;

namespace Project_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IDeveloperService _developerService;


        public AdminController(IProjectService projectService, IDeveloperService developerService)
        {
            _projectService = projectService;
            _developerService = developerService;
        }


        [HttpPut("add-developer-to-project/{projectId}/{developerId}")]
        public async Task<IActionResult> AddDeveloperToProject(Guid projectId, Guid developerId)
        {
            var project = _projectService.GetById(projectId);
            var developer = await _developerService.GetDeveloperByUserId(developerId);

            if (project == null || developer == null)
            {
                return NotFound();
            }

            _projectService.AddDeveloperToProject(project, developer);

            return Ok();
        }
    }

}
