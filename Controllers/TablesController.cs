using meta_menu_be.Common;
using meta_menu_be.JsonModels;
using meta_menu_be.Services.FoodCategoryService;
using meta_menu_be.Services.TablesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace meta_menu_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ITablesService tablesService;
        public TablesController(ITablesService tablesService)
        {
            this.tablesService = tablesService;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]   
        public ServiceResult<bool> Create([FromBody] TableJsonModel model)
        {
            string domainName = HttpContext.Request.Host.Value;
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = tablesService.Create(model.TableNumber, userId);

            return res;
        }

        [Authorize]
        [Route("get-all")]
        public ServiceResult<List<TableJsonModel>> GetAll()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = tablesService.GetAll(userId);

            return res;
        }

        [Authorize]
        [Route("download-qr")]
        public byte[] DownloadQr(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = tablesService.GetTableQrImage(id, userId);

            return res.Data;
        }

    }
}
