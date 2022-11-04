﻿using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.JsonModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace meta_menu_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext dbContext;
        private readonly AppSettings _appSettings;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            _configuration = configuration;
            this.dbContext = dbContext;
            this._appSettings = appSettings.Value;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var claimIdenifier = authClaims[1];

                var claimsIdenitity = new ClaimsIdentity(new[] { claimIdenifier }, "serverAuth");

                var token = GetToken(authClaims);


                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Data = user,
                    Success = true,
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return BadRequest(new ServiceResult<int>
                {
                    Status = "Failed",
                    Success = false,
                    Message = "User with that email already exists!",
                });
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                { UserName = model.Username, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new ServiceResult<int>
                    {
                        Status = "Success",
                        Success = true,
                        Message = "User registerd!",
                    });
                }
                else
                {
                    return BadRequest(new ServiceResult<int>
                    {
                        Status = "Failed",
                        Success = false,
                        Message = string.Join(", ", result.Errors),
                    });
                }
            }

            return BadRequest();
        }

        [Route("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser applicationUser = await userManager.FindByIdAsync(userId);

            return Ok(applicationUser);
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
       

    }
}

