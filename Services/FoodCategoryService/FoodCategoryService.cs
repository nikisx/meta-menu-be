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

        public ServiceResult<List<FoodCategoryJsonModel>> GetAll(string userId)
        {
            var res = dbContext.FoodCategories
                .Include(x => x.Items)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Created)
                .Select(x => new FoodCategoryJsonModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Items = x.Items.Select(i => new FoodItemJsonModel
                    {
                        Id = i.Id,
                        CategoryId = i.CategoryId,
                        Name = i.Name,
                    })
                }).ToList();

            return new ServiceResult<List<FoodCategoryJsonModel>>(res);
        }
    }
}
