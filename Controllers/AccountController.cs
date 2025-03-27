using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public readonly ITokenService _tokenService;

        private readonly SignInManager<AppUser> _signInManager;  // Dependency for signing in 

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;//Insatntitating the dependency 

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());//Finding User with USername if it exists or not
            if (user == null)
            {
                return Unauthorized("Invalid Username!");// If User does not exist
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);// false is for lockout failure , study for more 
            if (!result.Succeeded) return Unauthorized("Username not found and/or password is incorrect");
            return Ok(
                new NewUserDto { 
                    UserName = user.UserName,
                    Email  = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
                );


        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try { 
            if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser { 
                    UserName = registerDto.UserName,    
                    Email = registerDto.Email,

                };
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if(createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }

                            );
                        
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500,createdUser.Errors);
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
        }

    }
}
