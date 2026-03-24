using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.Core.Specification
{
    public class TaskSpecParams
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public int? ProjectId { get; set; }

        public string? Sort { get; set; }     // dateAsc / dateDesc
    }
}
