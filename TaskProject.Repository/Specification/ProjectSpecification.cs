using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Entites;
using TaskProject.Core.Specification;

namespace TaskProject.Repository.Specification
{
    public class ProjectSpecification : BaseSpecification<Project, int>
    {
        public ProjectSpecification(ProjectSpecParams param)
        {
            // Pagination
            ApplyPagination((param.PageIndex - 1) * param.PageSize, param.PageSize);

            // Sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "nameasc":
                        AddOrderBy(p => p.Name);
                        break;

                    case "namedesc":
                        AddOrderByDescending(p => p.Name);
                        break;

                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

            // Include
            Include.Add(p => p.Owner);
            Include.Add(p => p.Members);
        }
    }
}
