using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Enums;

namespace TaskProject.Core.Dtos.Task
{
    public class TaskCreateDto
    {
       
            public string Title { get; set; } = null!;
            public string? Description { get; set; }

            public int ProjectId { get; set; }
            public int AssignedToId { get; set; }

            public TaskStatus Status { get; set; }
            public TaskPriority Priority { get; set; }
        }
    }

