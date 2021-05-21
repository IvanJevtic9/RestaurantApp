namespace RestaurantApp.Core.Entity
{
    public class GalleryImage
    {
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        public int ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}
