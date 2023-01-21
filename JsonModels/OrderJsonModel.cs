using meta_menu_be.Entities;

namespace meta_menu_be.JsonModels
{
    public class OrderJsonModel
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int Type { get; set; }
        public string? UserId { get; set; }
        public bool? IsNew { get; set; }
        public string? TableNumber { get; set; }
        public string? Time { get; set; }
        public string? Price { get; set; }
        public List<FoodItemJsonModel>? Items { get; set; }
    }
}
