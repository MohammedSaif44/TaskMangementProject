using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.Core.Dtos.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;

        public int ProjectId { get; set; }          // ✅ ضيف دي
        public int? AssignedToId { get; set; }      // ✅ وضيف دي

        public string ProjectName { get; set; } = null!;
        public string? AssignedToUsername { get; set; }
    }
}
