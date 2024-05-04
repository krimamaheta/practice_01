using Practice_01.Models;
using Practice_01.ViewModel;

namespace Practice_01.Repository
{
    public interface IVendorRepository
    {
        Task<IEnumerable<VendorModel>> GetAllVendor();
        Task<VendorModel> GetVendorById(Guid vendorId);
        Task<VendorModel>AddVendorAsync(VendorModel newvendor, Guid UserId);
        Task<VendorModel>UpdateVendorAsync(Guid vendorId, VendorModel UpdateVendor);

        Task <bool> DeleteVendorAsync(Guid vendorId);

        //all type of vendor

        Task<List<string>> GetAlTypeOfvendor();

        //userId
        Task<VendorModel> GetVendorByUserId(string userId);



    }
}
