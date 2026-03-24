using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskProject.Core.Dtos.Project;
using TaskProject.Core.Dtos.Task;
using TaskProject.Core.Entites;
using TaskProject.Core.Services.Contract;
using TaskProject.Core.Specification;
using TaskProject.Service.Services;

namespace TaskMangementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService ProjectService;
        private readonly IMapper mapper;

        public ProjectController(IProjectService _ProjectService, IMapper mapper)
        {
            ProjectService = _ProjectService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects([FromQuery] ProjectSpecParams param)
        {
            var res = await ProjectService.GetAllProjectAsync(param);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int id)
        {
            var res = await ProjectService.GetProjectByIdAsync(id);
            if (res == null)
                return NotFound();

            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(ProjectCreateDto model)
        {
            var project = await ProjectService.CreateProjectAsync(model, model.OwnerId);

            if (project == null)
                return NotFound();

            return Ok(project);
        }
        [HttpPost("{projectId}/members/{userId}")]
        public async Task<IActionResult> AddMember(int projectId, int userId)
        {
            var result = await ProjectService.AddMemberAsync(projectId, userId);

            if (!result)
                return NotFound();

            return Ok("User added to project");
        }
    }
}
