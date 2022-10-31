namespace meta_menu_be.Entities
{
    public class FoodItem : BaseEntity
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual FoodCategory Category { get; set; }
    }
}
