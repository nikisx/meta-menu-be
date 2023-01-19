using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.Hubs;
using meta_menu_be.JsonModels;
using meta_menu_be.Services.OrdersService;
using meta_menu_be.Services.TablesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace meta_menu_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBaseExtended
    {   
        private IOrderService orderService;
        private IHubContext<OrderHub, IOrderHub> hubContext;
        public OrdersController(IOrderService orderService, IHubContext<OrderHub, IOrderHub> hubContext)
        {
            this.orderService = orderService;
            this.hubContext = hubContext;
        }

        [HttpPost]
        [Route("create")]
        public ServiceResult<OrderJsonModel> Create([FromBody] OrderJsonModel model)
        {
            var res = orderService.Create(model);

            if (res.Data != null)
            {
                hubContext.Clients.All.NewOrderRecieved(res.Data, model.UserId);
            }

            return res;
        }

        [HttpPost]
        [Route("finish")]
        public ServiceResult<bool> Finish([FromBody] OrderJsonModel model)
        {
            var res = orderService.Finish(model, GetLoggednInUserId());


            return res;
        }

        [HttpPost]
        [Route("open")]
        public ServiceResult<bool> ChangeOrderToOld([FromBody] OrderJsonModel model)
        {
            var res = orderService.ChangeOrderToOld(model, GetLoggednInUserId());

            return res;
        }

        [Route("get-all")]
        [Authorize]
        public ServiceResult<List<OrderJsonModel>> GetAll()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = orderService.GetAllForUser(userId);

            return res;
        }
    }
}
