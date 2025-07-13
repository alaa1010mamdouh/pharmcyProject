using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pharmcy.Core.Entities.Identity;
using Pharmcy.Core.Services;
using pharmcy_Project.DTO;
using pharmcy_Project.Errors;
using System.Formats.Asn1;

namespace pharmcy_Project.Controllers
{
    
    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _service;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            ,  ITokenServices service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _service = service;
        }
       
        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split('@')[0],
            };
          var result=  await _userManager.CreateAsync(User,model.Passsword);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            var returned = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token =await _service.CreatToken(User,_userManager)
            };
            return Ok(returned);
        }
        [HttpPost("login")]
        
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(User, loginDto.password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            var Retubed = new UserDto
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _service.CreatToken(User,_userManager)
            };
            return Ok(Retubed);
        }
    }
}
