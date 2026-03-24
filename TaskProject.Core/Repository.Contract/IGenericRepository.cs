using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Dtos.Project;
using TaskProject.Core.Entites;
using TaskProject.Core.PaginationResponses;
using TaskProject.Core.Specification;

namespace TaskProject.Core.Repository.Contract
{
    public interface IGenericRepository<TEntity,TKey> where TEntity:BaseEntity<TKey>
    {
        public Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity,TKey>spec);
        Task<int> GetCountAsync(ISpecification<TEntity, TKey> spec);
        public Task<TEntity> GetByIdAsync(int id);
        public Task AddAsync(TEntity entity);
        public void UpdateAsync(TEntity entity);
        public void DeleteAsync(TEntity entity);
    }
}
