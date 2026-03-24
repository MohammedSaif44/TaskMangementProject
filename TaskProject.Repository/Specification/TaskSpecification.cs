using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Entites;
using TaskProject.Core.Specification;

namespace TaskProject.Repository.Specification
{
    public class TaskSpecification : BaseSpecification<TaskItem, int>
    {
        public TaskSpecification(TaskSpecParams param)
        {
            // 🔹 Filtering على ProjectId فقط
            Expression<Func<TaskItem, bool>> criteria = t => true;

            if (param.ProjectId.HasValue)
            {
                criteria = t => t.ProjectId == param.ProjectId.Value;
            }

            Criteria = criteria;

            // 🔹 Sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "dateasc":
                        AddOrderBy(t => t.CreatedAt);
                        break;
                    case "datedesc":
                        AddOrderByDescending(t => t.CreatedAt);
                        break;
                    default:
                        AddOrderByDescending(t => t.CreatedAt);
                        break;
                }
            }
            else
            {
                AddOrderByDescending(t => t.CreatedAt);
            }

            // 🔹 Include
            Include.Add(t => t.Project);

            // 🔹 Pagination
            ApplyPagination((param.PageIndex - 1) * param.PageSize, param.PageSize);
        }
    }
}
