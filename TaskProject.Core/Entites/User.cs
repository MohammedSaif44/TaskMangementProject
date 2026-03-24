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
    public class User:BaseEntity<int>
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Role Role { get; set; }

        public ICollection<Project> ProjectsOwned { get; set; } = new List<Project>();
        public ICollection<Project> ProjectsMemberOf { get; set; } = new List<Project>();
        public ICollection<TaskItem> TasksAssigned { get; set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
