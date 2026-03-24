using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskProject.Core.Enums;

namespace TaskProject.Core.Entites
{
    public class Project:BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        // Owner
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        // Members (Many-to-Many)
        public ICollection<User> Members { get; set; } = new List<User>();

        // Tasks
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
