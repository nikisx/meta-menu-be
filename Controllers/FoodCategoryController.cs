using meta_menu_be.Common;
using meta_menu_be.JsonModels;
using meta_menu_be.Services.FoodCategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace meta_menu_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodCategoryController : ControllerBaseExtended
    {
        private IFoodCategoryService foodCategoryService;
        public FoodCategoryController(IFoodCategoryService foodCategoryService)
        {
            this.foodCategoryService = foodCategoryService;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public ServiceResult<bool> Create(FoodCategoryJsonModel model)
        {
            string userId = model.UserId is not null ? model.UserId : this.GetLoggednInUserId();
            var res = foodCategoryService.Create(model, userId);

            return res;
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public ServiceResult<bool> Edit(FoodCategoryJsonModel model)
        {
            string userId = model.UserId is not null ? model.UserId : this.GetLoggednInUserId();
            var res = foodCategoryService.Edit(model, userId);

            return res;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public ServiceResult<bool> Delete(FoodCategoryJsonModel model)
        {
            string userId = this.GetLoggednInUserId();
            var res = foodCategoryService.Delete(model, userId);

            return res;
        }

        [Authorize]
        [HttpPost]
        [Route("edit-hide")]
        public ServiceResult<bool> EditHide(FoodCategoryJsonModel model)
        {
            string userId = model.UserId is not null ? model.UserId : this.GetLoggednInUserId();
            var res = foodCategoryService.HideCategory(model, userId);

            return res;
        }

        [Route("get-all")]
        public ServiceResult<List<FoodCategoryJsonModel>> GetAll(string? userId)
        {
            string id = string.IsNullOrEmpty(userId) ? this.GetLoggednInUserId() : userId;
            var res = foodCategoryService.GetAll(id);

            return res;
        }
    }
}
