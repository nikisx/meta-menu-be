namespace meta_menu_be.JsonModels
{
    public class FoodItemJsonModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? Allergens { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageBytes { get; set; }
    }
}
