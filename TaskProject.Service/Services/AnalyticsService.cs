using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Analytics;
using TaskProject.Core.Entites;
using TaskProject.Core.Enums;
using TaskProject.Core.Repository.Contract;
using TaskProject.Core.Services.Contract;

namespace TaskProject.Service.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<TaskStatusAnalyticsDto> GetTaskStatusAnalyticsAll()
        {
            var tasks = await unitOfWork.Repositories<TaskItem, int>().GetAllAsync();
            return new TaskStatusAnalyticsDto
            {
                Todo = tasks.Count(t => t.Status == Taskstatus.ToDo),
                InProgress = tasks.Count(t => t.Status == Taskstatus.InProgress),
                Done = tasks.Count(t => t.Status == Taskstatus.Done)


            };
        }
    }
}
