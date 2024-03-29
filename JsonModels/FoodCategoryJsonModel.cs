﻿namespace meta_menu_be.JsonModels
{
    public class FoodCategoryJsonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }
        public bool IsHidden { get; set; }
        public bool IsOnFocus { get; set; }
        public IEnumerable<FoodItemJsonModel>? Items { get; set; }
    }
}
