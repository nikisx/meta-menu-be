using meta_menu_be.Entities;

namespace meta_menu_be.Hubs
{
    public interface IOrderHub
    {
        Task NewOrderRecieved(Order order, string userId);
    }
}
