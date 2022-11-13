using meta_menu_be.Common;
using meta_menu_be.JsonModels;
using meta_menu_be.Services.TablesService;
using meta_menu_be.Services.UsersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace meta_menu_be.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UsersController : ControllerBaseExtended
    {
        private readonly IUsersService usersService;
        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [Route("get-all")]
        public ServiceResult<List<UserJsonModel>> GetAll()
        {
            var res = usersService.GetAll();

            return res;
        }

        [AllowAnonymous]
        [Route("get-user-info")]
        public ServiceResult<UserJsonModel> GetUserInfo(string id)
        {
            var res = usersService.GetUserInfo(id);

            return res;
        }

        [Route("update-type")]
        [HttpPost]
        public ServiceResult<int> UpdateUserType(UserJsonModel model)
        {
            var res = usersService.UpdateUserAccountType(model.Id, model.AccountType, this.GetLoggednInUserId());

            return res;
        }
    }
}
