using meta_menu_be.Entities;
using meta_menu_be.JsonModels;

namespace meta_menu_be.Hubs
{
    public interface IOrderHub
    {
        Task NewOrderRecieved(OrderJsonModel order, string userId);
    }
}
