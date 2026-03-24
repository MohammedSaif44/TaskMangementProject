using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.Core.Dtos.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int OwnerId { get; set; }
        public string OwnerUsername { get; set; } = null!;

        public int MembersCount { get; set; }
        public int TasksCount { get; set; }
    }
}
