using Microsoft.AspNetCore.Identity;
using Practice_01.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Numerics;

namespace Practice_01.Authentication
{
    public class ApplicationUser:IdentityUser
    {
        public string Email { get; set; }

        //public string CreatedBy { get; set; } = "System";//store userid and username
        //public DateTime? CreatedDate { get; set; }//when record create
        //public string? UpdatedBy { get; set; }//user id and username
        //public DateTime? UpdatedDate { get; set; }//last update   

        //public Guid RoleId { get; set; }
        //[ForeignKey("RoleId")]
        //public virtual Role Role { get; set; }

        public bool IsActive { get; set; } = false;

        public virtual ICollection<Vendor> Vendor { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
