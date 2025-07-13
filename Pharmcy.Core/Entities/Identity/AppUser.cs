using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities.Identity
{
    public class AppUser:IdentityUser
    {
        public Address Address { get; set; }
        public string DisplayName { get; set; }
    }
}
