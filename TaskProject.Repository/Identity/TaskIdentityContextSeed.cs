using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProject.Core.Entites.Identity;

namespace TaskProject.Repository.Identity
{
    public class TaskIdentityContextSeed
    {
        public static async Task SeedAppUserAsync(UserManager <AppUser> usermanger)
        {
            if (usermanger.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "test123@gmail.com",
                    DisplayName = "Mohammedabdo",
                    UserName = "mohammed.abdo",
                    PhoneNumber = "01221555555",
                };
                await usermanger.CreateAsync(user,"P@ssw0rd");
            }   

        }
    }
}
