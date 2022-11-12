using meta_menu_be.Entities;

namespace meta_menu_be.JsonModels
{
    public class UserJsonModel
    {
        public string? Id { get; set; }
        public string? Username{ get; set; }
        public string? Email{ get; set; }

        public ICollection<string> Roles { get; set; }
        public virtual ICollection<FoodCategoryJsonModel> Categories { get; set; } 
        public virtual ICollection<TableJsonModel> Tables { get; set; } 
    }
}
