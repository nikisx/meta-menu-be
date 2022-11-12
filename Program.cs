
using meta_menu_be.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using meta_menu_be.Services.FoodCategoryService;
using meta_menu_be.Services.TablesService;
using meta_menu_be.Hubs;
using meta_menu_be.Services.OrdersService;
using meta_menu_be.Modules;
using meta_menu_be.Services.UsersService;

namespace meta_menu_be
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;

            // Add services to the container.

            // For Entity Framework
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

            // For Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();
            builder.Services.AddSignalR(x => x.EnableDetailedErrors = true);

            builder.Services.AddTransient<IFoodCategoryService, FoodCategoryService>();
            builder.Services.AddTransient<IFoodItemService, FoodItemService>();
            builder.Services.AddTransient<ITablesService, TablesService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IUsersService, UsersService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHub<OrderHub>("/api/orderHub");

            app.UseCors(builder =>
            {
                builder
                .WithOrigins("http://localhost:8080", "https://localhost:44349")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });

            SeedData.Initialize(app);

            app.UseHttpsRedirection();

            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}