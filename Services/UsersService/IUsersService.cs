using meta_menu_be.Common;
using meta_menu_be.JsonModels;

namespace meta_menu_be.Services.UsersService
{
    public interface IUsersService
    {
        ServiceResult<List<UserJsonModel>> GetAll();
    }
}
