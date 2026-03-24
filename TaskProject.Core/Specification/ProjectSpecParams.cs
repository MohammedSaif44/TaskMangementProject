using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProject.Core.Specification
{
    public class ProjectSpecParams
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? Sort { get; set; }
    }
}
