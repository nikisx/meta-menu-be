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

            var order = dbContext.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Item)
                .FirstOrDefault(x => x.TableNumber == table.Number && x.UserId == model.UserId && !x.IsFinished);

            if (order == null)
            {
                order = new Order()
                {
                    UserId = model.UserId,
                    TableNumber = table.Number,
                };

                dbContext.Orders.Add(order);
                dbContext.SaveChanges();

            }

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

            var resOrder = dbContext.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Item)
                .FirstOrDefault(x => x.Id == order.Id);

            var res = new OrderJsonModel
            {
                Id = resOrder.Id,
                UserId = resOrder.UserId,
                TableNumber = resOrder.TableNumber,
                Items = resOrder.Items.Select(i => new FoodItemJsonModel
                {
                    Id = i.Id,
                    Name = i.Item.Name,
                    Quantity = i.Quantity,
                    Price = i.Item.Price,
                }).ToList(),
                Time = resOrder.Created.Value.ToString("HH:mm"),
                Price = string.Format("{0:f2}", resOrder.Items.Sum(i => i.Item.Price * i.Quantity)),
            };

            return new ServiceResult<OrderJsonModel>(res);
        }

        public ServiceResult<bool> Finish(OrderJsonModel model, string userId)
        {
            var order = dbContext.Orders.FirstOrDefault(x => x.Id == model.Id);

            if (order == null)
            {
                return new ServiceResult<bool>("Invalid Id!");
            }

            order.IsFinished = true;
            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<List<OrderJsonModel>> GetAllForUser(string userId)
        {
            var res = dbContext.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Item)
                .Where(x => x.UserId == userId && !x.IsFinished)
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
