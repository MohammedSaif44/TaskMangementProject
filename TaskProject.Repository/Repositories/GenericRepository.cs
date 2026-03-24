
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Context;
using TaskProject.Core.Entites;
using TaskProject.Core.Repository.Contract;
using TaskProject.Core.Specification;
using TaskProject.Repository.Specification;

namespace TaskProject.Repository.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly TaskDbContext _context;

        public GenericRepository(TaskDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity entity)
        => await _context.Set<TEntity>().AddAsync(entity);

        public void DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            // Example: لو Entity محتاجة Include نحددها هنا
            if (typeof(TEntity) == typeof(TaskItem))
            {
                return (IEnumerable<TEntity>)await _context.Tasks
                    .Include(t => t.Project)
                    .Include(t => t.AssignedTo)
                    .Include(t => t.Comments)
                    .ToListAsync();
            }

            if (typeof(TEntity) == typeof(Project))
            {
                return (IEnumerable<TEntity>)await _context.Projects
                    .Include(p => p.Owner)
                    .Include(p => p.Members)
                    .Include(p => p.Tasks)
                    .ToListAsync();
            }

            if (typeof(TEntity) == typeof(Comment))
            {
                return (IEnumerable<TEntity>)await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.Task)
                    .ToListAsync();
            }
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> spec)
        {
            return await ApplySpecification(spec).ToListAsync();

        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            if (typeof(TEntity) == typeof(TaskItem))
            {
                return await _context.Tasks
                    .Include(t => t.Project)
                    .Include(t => t.AssignedTo)
                    .Include(t => t.Comments)
                    .FirstOrDefaultAsync(t => t.Id == id) as TEntity;
            }

            if (typeof(TEntity) == typeof(Project))
            {
                return await _context.Projects
                    .Include(p => p.Owner)
                    .Include(p => p.Members)
                    .Include(p => p.Tasks)
                    .FirstOrDefaultAsync(p => p.Id == id) as TEntity;
            }

            if (typeof(TEntity) == typeof(Comment))
            {
                return await _context.Comments
                    .Include(c => c.User)
                    .Include(c => c.Task)
                    .FirstOrDefaultAsync(c => c.Id == id) as TEntity;
            }

           return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(ISpecification<TEntity, TKey> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public void UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TKey> spec)
        {

            return SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), spec);
        }
    }
}
