using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Analytics;

namespace TaskProject.Core.Services.Contract
{
    public interface IAnalyticsService
    {
        Task<TaskStatusAnalyticsDto> GetTaskStatusAnalyticsAll();
    }
}
