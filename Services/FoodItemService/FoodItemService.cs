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
            MemoryStream memoryStream = new MemoryStream();
            model.Image?.CopyTo(memoryStream);

            var foodItem = new FoodItem()
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                Image = memoryStream.ToArray(),
                Description = model.Description,
                Allergens = model.Allergens,
                Price = double.Parse(model.Price),
            };

            dbContext.FoodItems.Add(foodItem);
            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<bool> Delete(FoodItemJsonModel model, string userId)
        {
            var foodItem = dbContext.FoodItems.FirstOrDefault(x => x.Id == model.Id);

            if (foodItem == null)
            {
                return new ServiceResult<bool>("Invalid Id!");
            }

            dbContext.FoodItems.Remove(foodItem);
            dbContext.SaveChanges();

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<bool> Edit(FoodItemJsonModel model, string userId)
        {
            var foodItem = dbContext.FoodItems.FirstOrDefault(x => x.Id == model.Id);

            if (foodItem == null)
            {
                return new ServiceResult<bool>("Invalid Id!");
            }

            foodItem.Name = model.Name;
            foodItem.Price = double.Parse(model.Price);
            foodItem.Description = model.Description;
            foodItem.Allergens = model.Allergens;

            if (model.Image != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                model.Image?.CopyTo(memoryStream);
                foodItem.Image = memoryStream.ToArray();
            }
            else if(model.ImageBytes == null)
            {
                foodItem.Image = null;
            }

            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<List<FoodItemJsonModel>> GetAll(string userId)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<bool> HideItem(FoodItemJsonModel model, string userId)
        {
            var item = dbContext.FoodItems.FirstOrDefault(c => c.Id == model.Id);
            item.IsHidden = model.IsHidden;

            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }
    }
}
