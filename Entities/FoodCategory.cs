namespace meta_menu_be.Entities
{
    public class FoodCategory : BaseEntity
    {
        public int Id { get; set; }
        public string  Name { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<FoodItem> Items { get; set; } = new HashSet<FoodItem>();
    }
}
