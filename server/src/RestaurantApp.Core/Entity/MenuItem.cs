namespace RestaurantApp.Core.Entity
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DishDescription { get; set; }
        public string TagPrice { get; set; }

        public int? ItemImageId { get; set; }
        public virtual Image ItemImage { get; set; }

        public int MenuId { get; set; }
        public virtual RestaurantMenu Menu {get;set;}
    }
}
