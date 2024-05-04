using System.ComponentModel.DataAnnotations;

namespace Practice_01.Models
{
    //public enum EventName
    //{
    //    WeddingCeremony,
    //    Haldi,
    //    Mehandi,
    //    Sangit,
    //    RingCeremony,
    //    BabyShower,
    //    AnnyversaryCeremony,
    //    BirthdayCelebration,
    //    ThreadCeremony
       
    //}
    public class Event
    {
        [Key]
        public Guid Id { get; set; }

        public string? CreatedBy { get; set; }//store userid and username
        public DateTime? CreatedDate { get; set; }//when record create
        public string? UpdatedBy { get; set; }//user id and username
        public DateTime? UpdatedDate { get; set; }//last update

        public string? EventName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<VendorEvent> VendorEvents { get; set; } = new List<VendorEvent>();

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
