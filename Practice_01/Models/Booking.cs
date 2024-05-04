using Microsoft.Extensions.Logging;
using Practice_01.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Numerics;

namespace Practice_01.Models
{
    public class Booking
    {

        [Key]
        public Guid Id { get; set; }

        public string? CreatedBy { get; set; }//store userid and username
        public DateTime? CreatedDate { get; set; }//when record create
        public string? UpdatedBy { get; set; }//user id and username
        public DateTime? UpdatedDate { get; set; }//last update

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public Guid EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Events { get; set; }


        public decimal? Payment { get; set; }
        public string? EventLocation { get; set; }
        public DateTime EventDate { get; set; }

        public bool IsBooked { get; set; } = false;
    }
}
