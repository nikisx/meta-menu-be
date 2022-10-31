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
    public class FoodCategoryController : ControllerBase
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
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = foodCategoryService.Create(model, userId);

            return res;
        }

        [Route("get-all")]
        public ServiceResult<List<FoodCategoryJsonModel>> GetAll(string? userId)
        {
            string id = string.IsNullOrEmpty(userId) ?  User.FindFirst(ClaimTypes.NameIdentifier)?.Value : userId;
            var res = foodCategoryService.GetAll(id);

            return res;
        }
    }
}
