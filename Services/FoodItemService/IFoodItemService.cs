using meta_menu_be.Common;
using meta_menu_be.JsonModels;

namespace meta_menu_be.Services.FoodCategoryService
{
    public interface IFoodItemService
    {
        ServiceResult<bool> Create(FoodItemJsonModel model, string userId);
        ServiceResult<List<FoodItemJsonModel>> GetAll(string userId);
    }
}
