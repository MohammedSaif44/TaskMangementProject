using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Analytics;
using TaskProject.Core.Dtos.Task;
using TaskProject.Core.Entites;
using TaskProject.Core.Enums;
using TaskProject.Core.PaginationResponses;
using TaskProject.Core.Specification;

namespace TaskProject.Core.Services.Contract
{
    public interface ITaskService
    {
        
        Task<PaginationResponse<TaskDto>> GetAllTasksAsync(TaskSpecParams param);

        Task<TaskDto?> GetTaskByIdAsync(int id);

        Task<TaskDto?> CreateTaskAsync(TaskCreateDto model);

        Task<bool> DeleteTaskAsync(int id);
        Task<bool> UpdateStatusAsync(int taskId, Taskstatus status);
       
    }
}
