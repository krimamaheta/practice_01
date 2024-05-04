using Practice_01.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Practice_01.Models
{
    public class Role
    {
        
            [Key]
            public Guid Id { get; set; }
        //public string CreatedBy { get; set; }//store userid and username
        //public DateTime CreatedDate { get; set; }//when record create
        //public string UpdatedBy { get; set; }//user id and username
        //public DateTime UpdatedDate { get; set; }//last update

        //public RoleName RoleName { get; set; }
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Decorator = "Decorator";
        public const string Caterer = "Caterer";

        //public virtual ICollection<ApplicationUser> Users { get; set; }

        
    }
}
