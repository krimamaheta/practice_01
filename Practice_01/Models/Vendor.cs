using Practice_01.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Practice_01.Models
{
    //public enum TypeOfVendor
    //{
    //    Caterer,
    //    Decorator
    //}

    //public enum District
    //{
    //    Gujarat, Rajsthan, Taminnadu, Tripura, Uttrakhand, AndhraPradesh, Assam, DadraNagarHaveli,
    //    Bihar, DamanandDiu, Goa, Hriyana, JammuKashmir, Karnataka, Ladakh, madhyaPradesh, Manipur, Mizoram,
    //    AndmanAndNicobarIslands, Odissa, Punjab, Sikkim, Telangana, UtterPradesh, westBengal,
    //    ArunachalPradesh,
    //    Chattishgarh,
    //    Delhi,
    //     HimachalPradesh, Jharkhand, Kerala, Lakshdweep, Maharastra, Meghalaya, Nagaland, Puducherry
    //}
    public class Vendor
    {
        [Key]
        public Guid Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public string? CreatedBy { get; set; }//store userid and username
        public DateTime? CreatedDate { get; set; }//when record create
        public string? UpdatedBy { get; set; }//user id and username
        public DateTime? UpdatedDate { get; set; }//last update



        public string? WebsiteUrl { get; set; }
        public string? Address { get; set; }
        public string? CityName { get; set; }

        public string? District { get; set; }

        public string? FirmName { get; set; }
        public string? TypeOfVendor { get; set; }

        public virtual ICollection<VendorEvent> VendorEvents { get; set; } =new List<VendorEvent>();
    }
}
