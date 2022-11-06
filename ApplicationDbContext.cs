using meta_menu_be.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

namespace meta_menu_be
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrdersItems { get; set; }

        public int SaveChanges(string? userId)
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added));

            foreach (var entity in entities)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    ((BaseEntity)entity.Entity).CreatedById = userId;
                }
                ((BaseEntity)entity.Entity).Created = System.DateTime.Now;

            }

            return SaveChanges();
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
