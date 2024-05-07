using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Practice_01.Models
{
    public class VendorEvent
    {
        [Key]
        public Guid Id { get; set; }

        public string? CreatedBy { get; set; }//store userid and username
        public DateTime? CreatedDate { get; set; }//when record create
        public string? UpdatedBy { get; set; }//user id and username
        public DateTime? UpdatedDate { get; set; }//last update


        public Guid VendorId { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }


        public Guid EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Events { get; set; }

        public decimal? Price { get; set; }

        //for caterer
        public string? DishName { get; set; }


        public virtual ICollection<Image> Images { get; set; }
        public string? CityName { get; internal set; }
        public string? District { get; internal set; }
    }
}
