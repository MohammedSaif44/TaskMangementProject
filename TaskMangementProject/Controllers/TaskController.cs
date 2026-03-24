using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskProject.Core.Dtos.Task;
using TaskProject.Core.Entites;
using TaskProject.Core.Enums;
using TaskProject.Core.Services.Contract;
using TaskProject.Core.Specification;
using TaskProject.Service.Services;

namespace TaskMangementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IMapper mapper;

        public TaskController(ITaskService _taskService, IMapper mapper)
        {
            taskService = _taskService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks([FromQuery] TaskSpecParams param)
        {
            var tasks = await taskService.GetAllTasksAsync(param);
            return Ok(tasks);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTaskById(int id)
        {
            var task = await taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }


        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(TaskCreateDto model)
        {
            var result = await taskService.CreateTaskAsync(model);

            if (result == null)
                return BadRequest("Invalid ProjectId or AssignedToId");

            return Ok(result);
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, Taskstatus status)
        {
            var result = await taskService.UpdateStatusAsync(id, status);

            if (!result)
                return NotFound("Task not found");

            return Ok(result);
        }
    }
}
