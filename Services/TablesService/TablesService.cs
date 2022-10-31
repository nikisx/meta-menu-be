using IronBarCode;
using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.JsonModels;
using System.Net;
using System.Net.Http.Headers;

namespace meta_menu_be.Services.TablesService
{
    public class TablesService : ITablesService
    {
        private ApplicationDbContext dbContext;
        public TablesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ServiceResult<bool> Create(int tableNumber, string userId)
        {
            var user = this.dbContext.Users.FirstOrDefault(x => x.Id == userId);

            for (int i = 0; i < tableNumber; i++)
            {
                var newTable = new Table
                {
                    UserId = userId,
                };

                this.dbContext.Add(newTable);
                this.dbContext.SaveChanges(userId);

               
                var link = $"http://localhost:8080/menu/{user.Id}/{newTable.Id}";

                GeneratedBarcode qr = BarcodeWriter.CreateBarcode(link, BarcodeEncoding.QRCode);
                var qrFileName = $"table-id-{newTable.Id}.png";

                string path = Path.Combine(Environment.CurrentDirectory, @$"qr-codes\tables-qr-codes\{user.UserName}\", qrFileName);
                qr.SaveAsPng(path);

                newTable.QrCodeUrl = path;
            }

            this.dbContext.SaveChanges(userId);
            return new ServiceResult<bool>(true);
        }

        public ServiceResult<List<TableJsonModel>> GetAll(string userId)
        {
           var res = this.dbContext.Tables
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Created)
                .Select(x => new TableJsonModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    QrUrl = x.QrCodeUrl,
                }).ToList();

            return new ServiceResult<List<TableJsonModel>>(res);
        }

        public ServiceResult<byte[]> GetTableQrImage(int tableId, string userId)
        {
            var table = dbContext.Tables.FirstOrDefault(x => x.Id == tableId);

            if (table == null)
            {
                return new ServiceResult<byte[]>("Incorect table Id!");
            }

            string path = table.QrCodeUrl;

            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StreamContent(new FileStream(path, FileMode.Open, FileAccess.Read));
            //response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            //response.Content.Headers.ContentDisposition.FileName = $"qr-code-{table.Number}";
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            //var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            var a = File.ReadAllBytes(path);

            return new ServiceResult<byte[]>(a);
        }
    }
}
    