using Practice_01.Authentication;
using Practice_01.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_01.ViewModel
{
    public class BookingModel
    {
        //f.k
        public string UserId { get; set; }
        //f.k
        public Guid EventId { get; set; }
        public decimal? Payment { get; set; }
        public string? EventLocation { get; set; }
        public DateTime EventDate { get; set; }

        public bool IsBooked { get; set; } = false;
    }
}
