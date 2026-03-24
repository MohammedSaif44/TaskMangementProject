using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskProject.Core.Enums;

namespace TaskProject.Core.Entites
{
    public class TaskItem:BaseEntity<int>
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Taskstatus Status { get; set; } = Taskstatus.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public int? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        // Navigation
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
