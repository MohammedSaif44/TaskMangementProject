using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Entites;

namespace TaskProject.Core.Repository.Contract
{
    public interface IUnitOfWork
    {
        public Task<int> completeAsync();
        public IGenericRepository<TEntity, TKey> Repositories<TEntity,TKey>() where TEntity:BaseEntity<TKey>;
    }
}
