using meta_menu_be.Common;
using meta_menu_be.JsonModels;

namespace meta_menu_be.Services.UsersService
{
    public class UsersService : IUsersService
    {
        private ApplicationDbContext dbContext;
        public UsersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ServiceResult<List<UserJsonModel>> GetAll()
        {
            var users = this.dbContext.Users.Select(x => new UserJsonModel
            {
                Id = x.Id,
                Username = x.UserName,
                Email = x.Email,
                AccountType = (int)x.AccountType,
            }).ToList();

            return new ServiceResult<List<UserJsonModel>>(users);
        }
    }
}
