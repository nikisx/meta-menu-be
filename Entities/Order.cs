﻿namespace meta_menu_be.Entities
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string TableNumber { get; set; }
    }
}
