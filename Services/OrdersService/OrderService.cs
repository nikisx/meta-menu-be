using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.JsonModels;
using Microsoft.EntityFrameworkCore;

namespace meta_menu_be.Services.OrdersService
{
    public class OrderService : IOrderService
    {
        private ApplicationDbContext dbContext;
        public OrderService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public ServiceResult<OrderJsonModel> Create(OrderJsonModel model)
        {
            var table = dbContext.Tables.FirstOrDefault(x => x.Id == model.TableId);

            if (table == null)
            {
                return new ServiceResult<OrderJsonModel>("Ïncorrect table Id!");
            }

            var order = new Order()
            {
                UserId = model.UserId,
                TableNumber = table.Number,
            };

            dbContext.Orders.Add(order);
            dbContext.SaveChanges();


            foreach (var item in model.Items)
            {
                var orderItems = new OrderItems
                {
                    ItemId = item.Id,
                    OrderId = order.Id,
                    Quantity = item.Quantity,
                };

                dbContext.OrdersItems.Add(orderItems);
            }

            dbContext.SaveChanges();

            var res = new OrderJsonModel
            {
                Id = order.Id,
                UserId = order.UserId,
                TableNumber = order.TableNumber,
                Items = model.Items,
                Time = order.Created.Value.ToString("HH:mm"),
                Price = string.Format("{0:f2}", order.Items.Sum(i => i.Item.Price * i.Quantity)),
            };

            return new ServiceResult<OrderJsonModel>(res);
        }

        public ServiceResult<List<OrderJsonModel>> GetAllForUser(string userId)
        {
            var res = dbContext.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Item)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Created)
                .Select(x => new OrderJsonModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    TableNumber = x.TableNumber,
                    Time = x.Created.Value.ToString("HH:mm"),
                    Price = string.Format("{0:f2}", x.Items.Sum(i => i.Item.Price * i.Quantity)),
                    Items = x.Items.Select(i => new FoodItemJsonModel
                    {
                        Id = i.Id,
                        Name = i.Item.Name,
                        Quantity = i.Quantity,
                        Price = i.Item.Price,
                    }).ToList()
                }).ToList();

           

            return new ServiceResult<List<OrderJsonModel>>(res);
        }
    }
}
