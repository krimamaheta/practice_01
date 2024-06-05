using Practice_01.Models;

namespace Practice_01.ViewModel
{
    public class VendorModel
    {
        public Guid? vendorId { get; set; }
        public string? UserId { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Address { get; set; }
        public string? CityName { get; set; }
        public string? District { get; set; }
        public string? FirmName { get; set; }
        public string? TypeOfVendor { get; set; }
        public bool? ISApprove { get; set; } = false;

        public char IsDeleted { get; set; } = 'A';

    }
}
