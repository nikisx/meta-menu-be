using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.JsonModels;
using Microsoft.EntityFrameworkCore;

namespace meta_menu_be.Services.FoodCategoryService
{
    public class FoodItemService : IFoodItemService
    {
        private ApplicationDbContext dbContext;
        public FoodItemService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public ServiceResult<bool> Create(FoodItemJsonModel model, string userId)
        {
            var foodItem = new FoodItem()
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
            };

            dbContext.FoodItems.Add(foodItem);
            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<List<FoodItemJsonModel>> GetAll(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
