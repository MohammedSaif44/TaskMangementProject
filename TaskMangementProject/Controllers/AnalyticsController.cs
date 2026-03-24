using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskProject.Core.Dtos.Analytics;
using TaskProject.Core.Services.Contract;
using TaskProject.Service.Services;

namespace TaskMangementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService analyticsService;

        public AnalyticsController(IAnalyticsService _analyticsService)
        {
           
            analyticsService = _analyticsService;
        }
        [HttpGet("task-status")]
        public async Task<ActionResult<TaskStatusAnalyticsDto>> GetTaskStatus()
        {
            var result = await analyticsService.GetTaskStatusAnalyticsAll();
            return Ok(result);
        }
    }
}
