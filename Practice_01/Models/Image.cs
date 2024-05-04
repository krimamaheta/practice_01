using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Practice_01.Models
{
    public class Image
    {

        [Key]
        public Guid Id { get; set; }

        public string? CreatedBy { get; set; }//store userid and username
        public DateTime? CreatedDate { get; set; }//when record create
        public string? UpdatedBy { get; set; }//user id and username
        public DateTime? UpdatedDate { get; set; }//last update

        //store image
        public string? ImageUrl { get; set; }
        // public decimal? Prices { get; set; }
       

        public Guid VendorEventId { get; set; }
        [ForeignKey("VendorEventId")]
        public virtual VendorEvent VendorEvent { get; set; }

    }
}
