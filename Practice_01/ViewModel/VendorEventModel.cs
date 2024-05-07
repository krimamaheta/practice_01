using Practice_01.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_01.ViewModel
{
    public class VendorEventModel
    {
        public Guid? Id { get; set; }
        public Guid VendorId { get; set; }
      
        public Guid EventId { get; set; }
        
        public decimal? Price { get; set; }
        public List<string>?Images { get; set; }

        //for decorator
        public string? FirmName { get; set; }
        public string? CityName { get; set; }
        public string? EventName { get; set; }
        public string? Address { get; set; }
        public string? District {  get; set; }
        public string ? WebsiteUrl { get; set; } 
        
        //for caterer
        public string ? DishName { get; set; }
        // public Vendor Vendor { get; set; }
        //public Event Events { get; set; }
        //public List<IFormFile>? Images {  get; set; }   

        //public virtual ICollection<Image> Images { get; set; }

    }
}
