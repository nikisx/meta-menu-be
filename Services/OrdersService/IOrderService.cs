using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.JsonModels;

namespace meta_menu_be.Services.OrdersService
{
    public interface IOrderService
    {
        ServiceResult<OrderJsonModel> Create(OrderJsonModel model);
        ServiceResult<List<OrderJsonModel>> GetAllForUser(string userId);
    }
}
