using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Entites;

namespace TaskProject.Core.Context
{
    public class TaskDbContext:DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // =======================
            // Many-to-Many: Project <-> User (Members)
            // =======================
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Members)
                .WithMany(u => u.ProjectsMemberOf)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectMember", // جدول وسيط
                    j => j.HasOne<User>()
                          .WithMany()
                          .HasForeignKey("UserId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Project>()
                          .WithMany()
                          .HasForeignKey("ProjectId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasKey("ProjectId", "UserId")
                );

            // =======================
            // One-to-Many: Project -> Tasks
            // =======================
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // =======================
            // One-to-Many: TaskItem -> Comments
            // =======================
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // =======================
            // One-to-Many: User -> TasksAssigned
            // =======================
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedTo)
                .WithMany(u => u.TasksAssigned)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull); // لو المستخدم اتشال، المهمة تبقى بدون AssignedTo

            // =======================
            // One-to-Many: User -> Comments
            // =======================
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =======================
            // One-to-Many: User -> ProjectsOwned
            // =======================
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.ProjectsOwned)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // Owner مينفعش يتحذف بسهولة
        }
        public DbSet<User> Users { get; set; } 
        public DbSet<Project> Projects { get; set; } 
        public DbSet<TaskItem> Tasks { get; set; } 
        public DbSet<Comment> Comments { get; set; }
    }
}
