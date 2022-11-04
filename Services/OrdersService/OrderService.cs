using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.JsonModels;

namespace meta_menu_be.Services.OrdersService
{
    public class OrderService : IOrderService
    {
        private ApplicationDbContext dbContext;
        public OrderService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public ServiceResult<Order> Create(int tableId, string userId)
        {
            var table = dbContext.Tables.FirstOrDefault(x => x.Id == tableId);

            if (table == null)
            {
                return new ServiceResult<Order>("Ïncorrect table Id!");
            }

            var order = new Order()
            {
                UserId = userId,
                TableNumber = table.Number,
            };

            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            return new ServiceResult<Order>(order);
        }

        public ServiceResult<List<OrderJsonModel>> GetAllForUser(string userId)
        {
            var res = dbContext.Orders
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Created)
                .Select(x => new OrderJsonModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    TableNumber = x.TableNumber,
                }).ToList();

            return new ServiceResult<List<OrderJsonModel>>(res);
        }
    }
}
