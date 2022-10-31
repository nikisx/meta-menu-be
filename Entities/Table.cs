namespace meta_menu_be.Entities
{
    public class Table : BaseEntity
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string? QrCodeUrl { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
