using meta_menu_be.Common;
using meta_menu_be.JsonModels;

namespace meta_menu_be.Services.UsersService
{
    public interface IUsersService
    {
        ServiceResult<List<UserJsonModel>> GetAll();
        ServiceResult<UserJsonModel> GetUserInfo(string id);
        ServiceResult<string> UpdateUserProfileImage(UserJsonModel model, string userId);
        ServiceResult<int> UpdateUserAccountType(string userId, int accountType, string loggedInUserId);
        ServiceResult<bool> DeleteAllOrders();
        ServiceResult<bool> DeleteUser(string userId, string loggedInUserId);
        ServiceResult<bool> EditUserInfo(string name, string wifi, string userId);
        ServiceResult<StatisticsJsonModel> GetStatistics(string userId);
    }
}
