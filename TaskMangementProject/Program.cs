
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskMangementProject.Helper;
using TaskProject.Core.Context;
using TaskProject.Core.Entites.Identity;
using TaskProject.Core.Mapping;
using TaskProject.Core.Repository.Contract;
using TaskProject.Core.Services.Contract;
using TaskProject.Repository.Identity;
using TaskProject.Repository.Identity.Context;
using TaskProject.Repository.UnitOfWork;
using TaskProject.Service.Email;
using TaskProject.Service.Services;
using TaskProject.Service.Services.Users;

namespace TaskMangementProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<TaskDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new TaskProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new UserProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new ProjectProfile()));

            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IUserAccountService, UserAccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddDbContext<TaskIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<TaskIdentityDbContext>()
            .AddDefaultTokenProviders();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
           await ApllySeeding.ApplySeedingAsync(app);

            app.MapControllers();

            app.Run();
        }
    }
}
