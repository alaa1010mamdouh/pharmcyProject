using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pharmcy.Core.Entities.Identity;
using Pharmcy.Core.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Services
{
    public class TokenService : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreatToken(AppUser User,UserManager<AppUser> userManager)
        {
            //payload
            //private Claiams 
            var Authclim = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,User.DisplayName),
                new Claim(ClaimTypes.Email,User.Email),
            };
            var Roles = await userManager.GetRolesAsync(User);
            foreach (var Role in Roles)
            {
                Authclim.Add(new Claim(ClaimTypes.Role, Role));
            }
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var Token = new JwtSecurityToken(

                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires:DateTime.Now.AddDays(double.Parse( _configuration["Jwt:DurationInDay"])),
                claims: Authclim,
                signingCredentials:new SigningCredentials(Key,SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        
    }
}
