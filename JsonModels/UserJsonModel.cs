using meta_menu_be.Entities;
using meta_menu_be.Enums;

namespace meta_menu_be.JsonModels
{
    public class UserJsonModel
    {
        public string? Id { get; set; }
        public string? Username{ get; set; }
        public string? Email{ get; set; }
        public string? Wifi{ get; set; }

        public int AccountType { get; set; }

        public ICollection<string>? Roles { get; set; }
        public virtual ICollection<FoodCategoryJsonModel>? Categories { get; set; } 
        public virtual ICollection<TableJsonModel>? Tables { get; set; } 
    }
}
