using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Management.Services;

namespace Project_Management.Controllers
{
    [Authorize(Roles = "Developer")]
    [ApiController]
    [Route("api/[controller]")]
    public class DeveloperController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IDeveloperService _deverloperService;

        public DeveloperController(IProjectService projectService, IDeveloperService developerService)
        {
            _projectService = projectService;
            _deverloperService = developerService;
        }

    }
}
