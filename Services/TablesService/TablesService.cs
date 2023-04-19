using meta_menu_be.Common;
using meta_menu_be.Entities;
using meta_menu_be.JsonModels;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Drawing;
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
            var user = this.dbContext.Users.Include(x => x.Tables).FirstOrDefault(x => x.Id == userId);

            var userTablesCount = user.Tables.Count;

            for (int i = userTablesCount; i < tableNumber + userTablesCount; i++)
            {
                var newTable = new Table
                {
                    UserId = userId,
                    Number = "Маса номер " + i,
                };

                this.dbContext.Add(newTable);
                this.dbContext.SaveChanges(userId);

               
                //var link = $"http://localhost:8080/menu/{user.Id}/{newTable.Id}";

                //var qrFileName = $"table-id-{newTable.Id}.png";
                //string path = Path.Combine(Environment.CurrentDirectory, @$"qr-codes\tables-qr-codes\{user.UserName}\", qrFileName);

                //QRCodeGenerator qrGenerator = new QRCodeGenerator();
                //QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
                //QRCode qrCode = new QRCode(qrCodeData);
                //Bitmap qrCodeImage = qrCode.GetGraphic(20);

                //Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, @$"qr-codes\tables-qr-codes\{user.UserName}"));

                //qrCodeImage.Save(path);

                //newTable.QrCodeUrl = path;
            }

            this.dbContext.SaveChanges(userId);
            return new ServiceResult<bool>(true);
        }

        public ServiceResult<bool> Delete(int tableId, string userId)
        {
            var table = dbContext.Tables.FirstOrDefault(x => x.Id == tableId);

            if (table is null)
            {
                return new ServiceResult<bool>("Table Id is incorrect!");
            }

            dbContext.Tables.Remove(table);
            dbContext.SaveChanges(userId);

            return new ServiceResult<bool>(true);
        }

        public ServiceResult<bool> Edit(int tableId, string number, string userId)
        {
            var table = dbContext.Tables.FirstOrDefault(x => x.Id == tableId);

            if (table is null)
            {
                return new ServiceResult<bool>("Table Id is incorrect!");
            }

            table.Number = number;

            dbContext.SaveChanges(userId);

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


            var link = $"https://meta-menu.netlify.app/menu/{userId}/{table.Id}";

            var qrFileName = $"table-id-{table.Id}.png";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            byte[] res = null;

            using (var stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                res = stream.ToArray();
            }

            return new ServiceResult<byte[]>(res);
        }
    }
}
    