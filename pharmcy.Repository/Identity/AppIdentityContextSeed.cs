using Microsoft.AspNetCore.Identity;
using Pharmcy.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmcy.Repository.Identity
{
    public static class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Alaa mamdouh",
                    Email = "alaamamdouh@gmail.com",
                    UserName = "alaamamdouh",
                    PhoneNumber = "011537375555"
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
        }
    }
}
