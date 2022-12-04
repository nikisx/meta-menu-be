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
    public class FoodItemController : ControllerBaseExtended
    {
        private readonly IFoodItemService foodItemService;
        public FoodItemController(IFoodItemService foodItemService)
        {
            this.foodItemService = foodItemService;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public ServiceResult<bool> Create([FromForm] FoodItemJsonModel model)
        {
            string userId = GetLoggednInUserId();
            var res = foodItemService.Create(model, userId);

            return res;
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public ServiceResult<bool> Edit([FromForm] FoodItemJsonModel model)
        {
            string userId = GetLoggednInUserId();
            var res = foodItemService.Edit(model, userId);

            return res;
        }

        [Authorize]
        [HttpPost]
        [Route("edit-hide")]
        public ServiceResult<bool> EditHide(FoodItemJsonModel model)
        {
            string userId = GetLoggednInUserId();
            var res = foodItemService.HideItem(model, userId);

            return res;
        }
    }
}
