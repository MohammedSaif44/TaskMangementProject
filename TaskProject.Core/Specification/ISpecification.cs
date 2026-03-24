using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Entites;

namespace TaskProject.Core.Specification
{
    public interface ISpecification<TEntity,TKey>where TEntity:BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { set; get; }
        public List<Expression<Func<TEntity, object>>> Include { set; get; }
        public Expression<Func<TEntity, object>> OrderBy { set; get; }
        public Expression<Func<TEntity, object>> OrderByDescending { set; get; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
