using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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
            var user = this.dbContext.Users
                .Include(x => x.Categories)
                .ThenInclude(x => x.Items)
                .Include(x => x.Tables)
                .FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
            {
                return Unauthorized("Грешно въведен имейл!");
            }

            if (await userManager.CheckPasswordAsync(user, model.Password))
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

                var token = GetToken(authClaims);

                HttpContext.Response.Cookies.Append("token", new JwtSecurityTokenHandler().WriteToken(token),
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7),
                        HttpOnly = true,
                        Secure = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None,
                    });

                return Ok(new
                {
                    //Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Data = MapUser(user, userRoles),
                    Success = true,
                });
            }
            return Unauthorized("Грешна парола!");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return Ok(new ServiceResult<int>
                {
                    Status = "Failed",
                    Success = false,
                    Message = "Потребител с този имейл адрес вече съществува!",
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

            ApplicationUser applicationUser = this.dbContext.Users
                .Include(x => x.Categories)
                .ThenInclude(x => x.Items)
                .Include(x => x.Tables)
                .FirstOrDefault(x => x.Id == userId);

            if (applicationUser == null)
            {
                return NoContent();
            }

            var roles = await userManager.GetRolesAsync(applicationUser);

            UserJsonModel user = MapUser(applicationUser, roles);

            return Ok(user);
        }

        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.Request.Cookies["token"] != null)
            {

                HttpContext.Response.Cookies.Append("token", "",
                      new CookieOptions
                      {
                          Expires = DateTime.Now.AddDays(-1),
                          HttpOnly = true,
                          Secure = true,
                          IsEssential = true,
                          SameSite = SameSiteMode.None,
                      });
            }

            return Ok(new ServiceResult<bool>
                    {
                        Status = "Success",
                        Success = true,
                        Message = "Success",
                    });
        }

        [Route("change-password")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ApplicationUser applicationUser = this.dbContext.Users
                .FirstOrDefault(x => x.Id == userId);

            if (applicationUser == null)
            {
                return BadRequest("Потребителят не е намерен!");
            }

            var res = await this.userManager.ChangePasswordAsync(applicationUser, model.OldPassword, model.NewPassword);

            if (res.Succeeded)
            {
                return Ok(new ServiceResult<bool>
                {
                    Status = "Success",
                    Success = true,
                    Message = "Success",
                });
            }
            else
            {
                return Ok(new ServiceResult<bool>
                {
                    Status = "Fail",
                    Success = false,
                    Message = "Нещо се обърка, моля опитайте отново.",
                });
            }
           
        }

        private static UserJsonModel MapUser(ApplicationUser applicationUser, IList<string> roles)
        {
            return new UserJsonModel
            {
                Id = applicationUser.Id,
                Username = applicationUser.UserName,
                Email = applicationUser.Email,
                Wifi = applicationUser.Wifi,
                Roles = roles,
                Categories = applicationUser.Categories.OrderByDescending(x => x.Created).Select(x => new FoodCategoryJsonModel
                {
                    Name = x.Name,
                    Id = x.Id,
                    Items = x.Items.Select(i => new FoodItemJsonModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        CategoryId = i.CategoryId,
                    }).ToList(),
                }).ToList(),
                Tables = applicationUser.Tables.Select(x => new TableJsonModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    QrUrl = x.QrCodeUrl,

                }).ToList(),
                AccountType = (int)applicationUser.AccountType,
            };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
       

    }
}

