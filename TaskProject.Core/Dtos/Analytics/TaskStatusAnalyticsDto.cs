using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.Core.Dtos.Analytics
{
    public class TaskStatusAnalyticsDto
    {
        public int Todo { get; set; }
        public int InProgress { get; set; }
        public int Done { get; set; }
    }
}
