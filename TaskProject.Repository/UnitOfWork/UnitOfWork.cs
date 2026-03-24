using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Context;
using TaskProject.Core.Entites;
using TaskProject.Core.Repository.Contract;
using TaskProject.Repository.Repositories;

namespace TaskProject.Repository.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly TaskDbContext _context;
        private readonly Hashtable _repositories;

        public UnitOfWork(TaskDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        public async Task<int> completeAsync()
     => await _context.SaveChangesAsync();

        public IGenericRepository<TEntity, TKey> Repositories<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositories= new GenericRepository<TEntity, TKey>(_context);
                _repositories.Add(type, repositories);
            }
            return _repositories[type] as GenericRepository<TEntity,TKey>;
        }
    }
}
