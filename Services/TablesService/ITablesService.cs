using meta_menu_be.Common;
using meta_menu_be.JsonModels;
using System.IO;

namespace meta_menu_be.Services.TablesService
{
    public interface ITablesService
    {
        ServiceResult<bool> Create(int tableNumber, string userId);
        ServiceResult<bool> Edit(int tableId, string number, string userId);
        ServiceResult<byte[]> GetTableQrImage(int tableId, string userId);
        ServiceResult<List<TableJsonModel>> GetAll(string userId);
    }
}
