using meta_menu_be.Common;
using meta_menu_be.JsonModels;

namespace meta_menu_be.Services.FoodCategoryService
{
    public interface IFoodCategoryService
    {
        ServiceResult<bool> Create(FoodCategoryJsonModel model, string userId);
        ServiceResult<bool> Edit(FoodCategoryJsonModel model, string userId);
        ServiceResult<bool> HideCategory(FoodCategoryJsonModel model, string userId);
        ServiceResult<List<FoodCategoryJsonModel>> GetAll( string userId);
    }
}
