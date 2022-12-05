using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.JsonModels;
using Microsoft.EntityFrameworkCore;

namespace meta_menu_be.Services.FoodCategoryService
{
    public class FoodCategoryService : IFoodCategoryService
    {
        private ApplicationDbContext dbContext;
        public FoodCategoryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public ServiceResult<bool> Create(FoodCategoryJsonModel model, string userId)
        {
            var foodCategory = new FoodCategory()
            {
                Name = model.Name,
                UserId = userId,
            };

            dbContext.FoodCategories.Add(foodCategory);
            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<bool> Delete(FoodCategoryJsonModel model, string userId)
        {
            var category = dbContext.FoodCategories.FirstOrDefault(c => c.Id == model.Id);

            if (category == null)
            {
                return new ServiceResult<bool>("Invalid id");
            }

            dbContext.FoodCategories.Remove(category);
            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<bool> Edit(FoodCategoryJsonModel model, string userId)
        {
            var category = dbContext.FoodCategories.FirstOrDefault(c => c.Id == model.Id);
            category.Name = model.Name;

            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<List<FoodCategoryJsonModel>> GetAll(string userId)
        {
            var res = dbContext.FoodCategories
                .Include(x => x.Items)
                .Where(x => x.UserId == userId)
                .Select(x => new FoodCategoryJsonModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Items = x.Items.Select(i => new FoodItemJsonModel
                    {
                        Id = i.Id,
                        CategoryId = i.CategoryId,
                        Name = i.Name,
                        ImageBytes = i.Image.Length > 0 ? Convert.ToBase64String(i.Image) : null,
                        Description = i.Description,
                        Price = i.Price,
                        Allergens = i.Allergens,
                        IsHidden = i.IsHidden,
                    }),
                    IsHidden = x.IsHidden,
                }).ToList();

            return new ServiceResult<List<FoodCategoryJsonModel>>(res);
        }

        public ServiceResult<bool> HideCategory(FoodCategoryJsonModel model, string userId)
        {
            var category = dbContext.FoodCategories.FirstOrDefault(c => c.Id == model.Id);
            category.IsHidden = model.IsHidden;

            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }
    }
}
