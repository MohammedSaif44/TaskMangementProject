using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.Core.Dtos.Project
{
    public class ProjectCreateDto
    {
            public string Name { get; set; } = null!;
            public string? Description { get; set; }

           public int OwnerId { get; set; }
        
    }
}
