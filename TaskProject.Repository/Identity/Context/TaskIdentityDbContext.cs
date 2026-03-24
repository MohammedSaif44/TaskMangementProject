using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Entites.Identity;

namespace TaskProject.Repository.Identity.Context
{
    public class TaskIdentityDbContext:IdentityDbContext<AppUser>
    {
        public TaskIdentityDbContext(DbContextOptions<TaskIdentityDbContext> options):base(options)
        {
            
        }

    }
}
