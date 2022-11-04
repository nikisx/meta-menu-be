﻿using meta_menu_be.Common;
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
    public class OrdersController : ControllerBase
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
        public ServiceResult<Order> Create([FromBody] OrderJsonModel model)
        {
            var res = orderService.Create(model.TableId, model.UserId);

            if (res.Data != null)
            {
                hubContext.Clients.All.NewOrderRecieved(res.Data, model.UserId);
            }

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
