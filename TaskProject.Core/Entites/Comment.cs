using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.Core.Entites
{
    public class Comment:BaseEntity<int>
    {
        public string Content { get; set; } = null!;

        public int TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
