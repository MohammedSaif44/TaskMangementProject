using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskProject.Core.Context;
using TaskProject.Core.Entites;
using TaskProject.Core.Enums;

namespace TaskProject.Repository.Seeding
{
    public  class TaskContextSeed
    {
        //public static async Task seedAsync(TaskDbContext context,ILoggerFactory loggerFactory)
        //{
        //    try
        //    {
        //        // 1️⃣ Users
        //        if (!context.Users.Any())
        //        {
        //            var usersData = File.ReadAllText("../TaskProject.Repository/SeedData/User.json");
        //            var users = JsonSerializer.Deserialize<List<User>>(usersData);

        //            if (users != null)
        //            {
        //                await context.Users.AddRangeAsync(users);
        //                await context.SaveChangesAsync();
        //            }
        //        }

        //        // 2️⃣ Projects
        //        if (!context.Projects.Any())
        //        {
        //            var projectsData = File.ReadAllText("../TaskProject.Repository/SeedData/Projects.json");
        //            var projects = JsonSerializer.Deserialize<List<Project>>(projectsData);

        //            if (projects != null)
        //            {
        //                // Map Members manually by UserId
        //                foreach (var project in projects)
        //                {
        //                    var members = new List<User>();
        //                    if (project.Members != null && project.Members.Count > 0)
        //                    {
        //                        foreach (var userId in project.Members)
        //                        {
        //                            var user = await context.Users.FindAsync(userId);
        //                            if (user != null) members.Add(user);
        //                        }
        //                    }
        //                    project.Members = members;
        //                }

        //                await context.Projects.AddRangeAsync(projects);
        //                await context.SaveChangesAsync();
        //            }
        //        }

        //        // 3️⃣ Tasks
        //        if (context.Tasks != null && !context.Tasks.Any())
        //        {
        //            var tasksData = File.ReadAllText(@"C:\Full\Path\To\Tasks.json");
        //            Console.WriteLine(tasksData.Substring(0, 100)); // تأكد إن الملف اتقرا

        //            //var tasksData = File.ReadAllText("../TaskProject.Repository/SeedData/Tasks.json");
        //            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(tasksData);
        //            if (tasks == null)
        //            {
        //                Console.WriteLine("Failed to deserialize Tasks.json!");
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Loaded {tasks.Count} tasks from JSON.");
        //            }
        //            if (tasks != null)
        //            {
        //                await context.Tasks.AddRangeAsync(tasks);
        //                await context.SaveChangesAsync();
        //            }
        //        }

        //        // 4️⃣ Comments
        //        if (!context.Comments.Any())
        //        {
        //            var commentsData = File.ReadAllText("../TaskProject.Repository/SeedData/Comments.json");
        //            var comments = JsonSerializer.Deserialize<List<Comment>>(commentsData);
        //            if (comments != null)
        //            {
        //                await context.Comments.AddRangeAsync(comments);
        //                await context.SaveChangesAsync();
        //            }
        //        }

        //        Console.WriteLine("Database seeded successfully from JSON!");
        //    }
        //    catch (Exception ex)
        //    {
        //        var logger = loggerFactory.CreateLogger<TaskDbContext>();
        //        logger.LogError(ex, "An error occurred during JSON seeding");
        //    }
        //}

        public static async Task SeedAsync(TaskDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                // 1️⃣ Users
                if (!await context.Users.AnyAsync())
                {
                    var users = new List<User>
                    {
                        new User { Username = "alice", Email = "alice@example.com", PasswordHash = "hashed1", Role = 0, CreatedAt = DateTime.Now },
                        new User { Username = "bob", Email = "bob@example.com", PasswordHash = "hashed2", Role = 0, CreatedAt = DateTime.Now },
                        new User { Username = "charlie", Email = "charlie@example.com", PasswordHash = "hashed3", Role = 0, CreatedAt = DateTime.Now }
                    };

                    context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                }

                // 2️⃣ Projects
                if (!await context.Projects.AnyAsync())
                {
                    var alice = await context.Users.FirstAsync(u => u.Username == "alice");
                    var bob = await context.Users.FirstAsync(u => u.Username == "bob");
                    var charlie = await context.Users.FirstAsync(u => u.Username == "charlie");

                    var projects = new List<Project>
                    {
                        new Project
                        {
                            Name = "Project Alpha",
                            Description = "First project",
                            OwnerId = alice.Id,
                            Owner=alice,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        },
                        new Project
                        {
                            Name = "Project Beta",
                            Description = "Second project",
                            OwnerId = bob.Id,
                             Owner=bob,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        }
                    };

                    context.Projects.AddRangeAsync(projects);
                    await context.SaveChangesAsync();
                }

                // 3️⃣ Tasks
                if (!await context.Tasks.AnyAsync())
                {
                    var alphaProject = await context.Projects.FirstAsync(p => p.Name == "Project Alpha");
                    var betaProject = await context.Projects.FirstAsync(p => p.Name == "Project Beta");
                    var alice = await context.Users.FirstAsync(u => u.Username == "alice");
                    var bob = await context.Users.FirstAsync(u => u.Username == "bob");

                    var tasks = new List<TaskItem>
                    {
                        new TaskItem
                        {
                            Title = "Design Homepage",
                            Description = "Create homepage design",
                            ProjectId = alphaProject.Id,
                            AssignedToId = alice.Id,
                            Status = 0,
                            Priority = 0,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        },
                        new TaskItem
                        {
                            Title = "API Integration",
                            Description = "Connect APIs",
                            ProjectId = betaProject.Id,
                            AssignedToId = bob.Id,
                            Status = 0,
                            Priority = 0,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        }
                    };

                    context.Tasks.AddRangeAsync(tasks);
                    await context.SaveChangesAsync();
                }

                Console.WriteLine("Database seeded successfully!");
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<TaskContextSeed>();
                logger.LogError(ex, "An error occurred during seeding.");
            }
        }

    }
}
