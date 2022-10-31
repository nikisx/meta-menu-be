using Microsoft.AspNetCore.Identity;

namespace meta_menu_be.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<FoodCategory> Categories { get; set; } = new HashSet<FoodCategory>();
        public virtual ICollection<Table> Tables { get; set; } = new HashSet<Table>();
    }
}
