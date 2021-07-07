namespace RestaurantApp.Web.WebModels
{
    public class ApiResponse
    {
        public dynamic Errors { get; set; }
        public dynamic Data { get; set; }
        public dynamic Message { get; set; }
    }
}
