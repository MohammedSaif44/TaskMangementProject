using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Entites;
using TaskProject.Core.Specification;

namespace TaskProject.Repository.Specification
{
    public class SpecificationEvaluator<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputquery, ISpecification<TEntity, Tkey> spec)
        {
            var query = inputquery;
            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            if (spec.Include is not null && spec.Include.Any())
            {
                query = spec.Include.Aggregate(query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            }


            return query;
        }

    }
}
