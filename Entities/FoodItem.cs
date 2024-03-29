﻿namespace meta_menu_be.Entities
{
    public class FoodItem : BaseEntity
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public bool IsHidden { get; set; }
        public int CategoryId { get; set; }
        public virtual FoodCategory Category { get; set; }
        public double Price{ get; set; }
        public string? Description { get; set; }
        public string? Allergens { get; set; }
        public byte[]? Image { get; set; }
        public virtual ICollection<OrderItems> Orders { get; set; } = new HashSet<OrderItems>();
    }
}
