using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskProject.Core;
using TaskProject.Core.Context;
using TaskProject.Core.Entites.Identity;
using TaskProject.Repository.Identity;
using TaskProject.Repository.Identity.Context;
using TaskProject.Repository.Seeding;
namespace TaskMangementProject.Helper
{
    public class ApllySeeding
    {
        public static async Task ApplySeedingAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var Loggerfactory = services.GetRequiredService<ILoggerFactory>();


                try
                {
                    var context = services.GetRequiredService<TaskDbContext>();
                    var Identitycontext = services.GetRequiredService<TaskIdentityDbContext>();
                    var usermanger = services.GetRequiredService<UserManager<AppUser>>();
                    await context.Database.MigrateAsync();
                    await Identitycontext.Database.MigrateAsync();
                    await TaskContextSeed.SeedAsync(context, Loggerfactory);
                    await TaskIdentityContextSeed.SeedAppUserAsync(usermanger);

                }
                catch (Exception ex)
                {
                    var looger = Loggerfactory.CreateLogger<ApllySeeding>();
                    looger.LogError(ex.Message);

                }

            }
        }
    }
}
