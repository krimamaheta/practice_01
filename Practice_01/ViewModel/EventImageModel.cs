namespace Practice_01.ViewModel
{
    //event and image upload 
    public class EventImageModel
    {
        public EventModel EventModel { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile FormFile { get; set; }
        public decimal? Prices { get; set; }
        //fk
        public Guid VendorEventId { get; set; }
    }
}
