using meta_menu_be.Common;
using meta_menu_be.JsonModels;
using meta_menu_be.Services.TablesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace meta_menu_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;
        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [Authorize("Adminj")]
        [Route("get-all")]
        public ServiceResult<List<UserJsonModel>> GetAll()
        {
            var res = usersService.GetAll();

            return res;
        }
    }
}
