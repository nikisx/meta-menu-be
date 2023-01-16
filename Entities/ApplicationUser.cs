using meta_menu_be.Enums;
using Microsoft.AspNetCore.Identity;

namespace meta_menu_be.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int LastMonthOrdersCount { get; set; }
        public int CurrentMonthOrdersCount { get; set; }
        public byte[]? ImageBytes { get; set; }
        public string? Wifi { get; set; }
        public AccountType AccountType { get; set; }
        public virtual ICollection<FoodCategory> Categories { get; set; } = new HashSet<FoodCategory>();
        public virtual ICollection<Table> Tables { get; set; } = new HashSet<Table>();
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
