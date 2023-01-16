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
    [ApiController]
    public class UsersController : ControllerBaseExtended
    {
        private readonly IUsersService usersService;
        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [Route("get-all")]
        [Authorize(Roles = "Admin")]
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

        [Route("edit-user-info")]
        [HttpPost]
        public ServiceResult<bool> EditUserName([FromForm] UserJsonModel model)
        {
            var res = usersService.EditUserInfo(model.Username, model.Wifi, this.GetLoggednInUserId());

            return res;
        }

        [Route("update-image")]
        [HttpPost]
        public ServiceResult<bool> UpdateUserImage([FromForm] UserJsonModel model)
        {
            var res = usersService.UpdateUserProfileImage(model, this.GetLoggednInUserId());

            return res;
        }

        [Route("update-type")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ServiceResult<int> UpdateUserType(UserJsonModel model)
        {
            var res = usersService.UpdateUserAccountType(model.Id, model.AccountType, this.GetLoggednInUserId());

            return res;
        }

        [Route("delete-orders")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ServiceResult<bool> DeleteAllOrders()
        {
            var res = usersService.DeleteAllOrders();

            return res;
        }

        [Route("delete")]
        [HttpPost]
        public ServiceResult<bool> DeleteAllOrders([FromBody] UserJsonModel model)
        {
            var res = usersService.DeleteUser(model.Id, GetLoggednInUserId());

            return res;
        }

        [Route("get-statistics")]
        [Authorize]
        public ServiceResult<StatisticsJsonModel> GetStatistics()
        {
            var userId = GetLoggednInUserId();

            var res = usersService.GetStatistics(userId);

            return res;
        }
    }
}
