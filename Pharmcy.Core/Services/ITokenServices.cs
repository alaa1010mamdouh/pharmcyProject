using Microsoft.AspNetCore.Identity;
using Pharmcy.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Services
{
    public interface ITokenServices
    {
        Task<string>CreatToken(AppUser User, UserManager<AppUser> userManager);
    }
}
