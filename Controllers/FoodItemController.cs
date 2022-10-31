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
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemService foodItemService;
        public FoodItemController(IFoodItemService foodItemService)
        {
            this.foodItemService = foodItemService;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public ServiceResult<bool> Create(FoodItemJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = foodItemService.Create(model, userId);

            return res;
        }
    }
}
