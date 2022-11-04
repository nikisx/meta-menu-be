using meta_menu_be.Entities;

namespace meta_menu_be.JsonModels
{
    public class OrderJsonModel
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string? UserId { get; set; }

        public string? TableNumber { get; set; }
    }
}
