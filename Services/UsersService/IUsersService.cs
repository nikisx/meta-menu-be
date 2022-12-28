﻿using meta_menu_be.Common;
using meta_menu_be.JsonModels;

namespace meta_menu_be.Services.UsersService
{
    public interface IUsersService
    {
        ServiceResult<List<UserJsonModel>> GetAll();
        ServiceResult<UserJsonModel> GetUserInfo(string id);
        ServiceResult<int> UpdateUserAccountType(string userId, int accountType, string loggedInUserId);
        ServiceResult<bool> DeleteAllOrders();
        ServiceResult<bool> EditUserName(string name, string userId);
        ServiceResult<StatisticsJsonModel> GetStatistics(string userId);
    }
}
