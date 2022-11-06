using meta_menu_be.Entities;
using meta_menu_be.JsonModels;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace meta_menu_be.Hubs
{
    public class OrderHub: Hub<IOrderHub>
    {
        public async Task NewOrderRecieved(OrderJsonModel order, string userId)
        {
            await Clients.Group(userId).NewOrderRecieved(order, userId);
        }
        public override Task OnConnectedAsync()
        {
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            
            return base.OnConnectedAsync();
        }

    }
}
