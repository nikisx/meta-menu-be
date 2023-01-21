﻿using meta_menu_be.Enums;

namespace meta_menu_be.Entities
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public bool IsNew { get; set; }
        public string TableNumber { get; set; }
        public OrderType Type { get; set; }
        public bool IsFinished { get; set; }
        public virtual ICollection<OrderItems> Items { get; set; } = new HashSet<OrderItems>();
    }
}
