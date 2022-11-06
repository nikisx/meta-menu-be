namespace meta_menu_be.Entities
{
    public class OrderItems
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }

        public int? ItemId { get; set; }
        public virtual FoodItem? Item { get; set; }

        public int Quantity { get; set; }
    }
}
