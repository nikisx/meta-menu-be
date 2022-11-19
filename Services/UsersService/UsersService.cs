﻿using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.Enums;
using meta_menu_be.JsonModels;
using Microsoft.EntityFrameworkCore;

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

        public ServiceResult<UserJsonModel> GetUserInfo(string id)
        {
            var user = this.dbContext.Users
                .Include(x => x.Categories)
                .ThenInclude(x => x.Items)
                .Include(x => x.Tables)
                .FirstOrDefault(x => x.Id == id);

            if (user is null)
            {
                return new ServiceResult<UserJsonModel>("Invalid user Id !");
            }

            return new ServiceResult<UserJsonModel>(MapUser(user));

        }

        public ServiceResult<int> UpdateUserAccountType(string userId, int accountType, string loggedInUserId)
        {
            var user = this.dbContext.Users.FirstOrDefault(x => x.Id == userId);

            if (user is null)
            {
                return new ServiceResult<int>("Invalid user Id !");
            }

            user.AccountType = (AccountType)accountType;

            dbContext.SaveChanges(loggedInUserId);

            return new ServiceResult<int>(accountType);
        }

        private static UserJsonModel MapUser(ApplicationUser applicationUser)
        {
            return new UserJsonModel
            {
                Id = applicationUser.Id,
                Username = applicationUser.UserName,
                Email = applicationUser.Email,
                Categories = applicationUser.Categories.Select(x => new FoodCategoryJsonModel
                {
                    Name = x.Name,
                    Id = x.Id,
                    Items = x.Items.Select(i => new FoodItemJsonModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        CategoryId = i.CategoryId,
                    }).ToList(),
                }).ToList(),
                Tables = applicationUser.Tables.Select(x => new TableJsonModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    QrUrl = x.QrCodeUrl,

                }).ToList(),
                AccountType = (int)applicationUser.AccountType,
            };
        }

        public ServiceResult<bool> DeleteAllOrders()
        {
            var orderItems = this.dbContext.OrdersItems.ToList();
            var orders = this.dbContext.Orders.ToList();

            dbContext.OrdersItems.RemoveRange(orderItems);
            dbContext.Orders.RemoveRange(orders);

            dbContext.SaveChanges();

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<StatisticsJsonModel> GetStatistics(string userId)
        {
            var user = this.dbContext.Users
                .Include(x => x.Orders)
                .FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return new ServiceResult<StatisticsJsonModel>("Invalid User Id!");
            }

            var userOrders = user.Orders;

            double morgingCount = userOrders.Count(x => x.Created.Value.Hour < 11);
            double lunchCount = userOrders.Count(x => x.Created.Value.Hour >= 11 && x.Created.Value.Hour < 15);
            double afternoonCount = userOrders.Count(x => x.Created.Value.Hour >= 15 && x.Created.Value.Hour < 18);
            double eveningCount = userOrders.Count(x => x.Created.Value.Hour >= 18);

            double moringPercent = (morgingCount / userOrders.Count) * 100;
            double lunchPercent = (lunchCount / userOrders.Count) * 100;
            double afternoonPercent = (afternoonCount / userOrders.Count) * 100;
            double eveningPercent = (eveningCount / userOrders.Count) * 100;

            return new ServiceResult<StatisticsJsonModel>(new StatisticsJsonModel
            {
                Morning = moringPercent,
                Lunch = lunchPercent,
                Afternoon = afternoonPercent,
                Evening = eveningPercent,
            });
        }
    }
}
