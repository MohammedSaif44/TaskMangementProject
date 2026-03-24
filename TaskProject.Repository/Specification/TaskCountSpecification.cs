using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TaskProject.Core.Specification;
using TaskProject.Core.Entites;

namespace TaskProject.Repository.Specification
{


    public class TaskCountSpecification : BaseSpecification<TaskItem, int>
    {
        public TaskCountSpecification(TaskSpecParams param)
        {
            // 🔹 Filter على ProjectId فقط
            if (param.ProjectId.HasValue)
            {
                Criteria = t => t.ProjectId == param.ProjectId.Value;
            }
            else
            {
                Criteria = t => true; // لو مفيش ProjectId، عد كل الـ Tasks
            }

            // مفيش sorting أو pagination لأننا بس عايزين count
            // Include مش مهم هنا
        }
    }
}
