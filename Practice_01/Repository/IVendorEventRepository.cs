using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Practice_01.Models;
using Practice_01.ViewModel;

namespace Practice_01.Repository
{
    public interface IVendorEventRepository
    {
        Task<bool> AddVendorEvent(VendorEventModel viewModel);
        Task<bool> AddImage(ImageModel viewModel);
        IQueryable<ImageModel> GetAllImagesWithPrices();
        // IQueryable<VendorEvent> GetVendorEvents(string userSelectedCity, string userSelectedEventName, decimal userGivenPrice);

        //add event image work well
        Task<bool> AddEventvendorDetails(VendorEventModel viewModel, Cloudinary cloudinary);

        //get all
        Task<List<VendorEventModel>> GetAllEventvendorDetails();

        //getall with list
        List<VendorEventModel> GetAllVendorEventImages();

        //delete by eventvendorid
        Task<bool> DeleteVendorById(Guid EventVendorId);


        //get all base on vendorID
        Task<List<VendorEventModel>>  GetAllVendorId(Guid vendorId);

        //update all 
        Task<VendorEvent> UpdateVendorEvent(Guid vendorId, VendorEventModel model);


        Task<VendorEventModel>GetvendorEventById(Guid Id);

        //linq

        //get alllist city
        Task<List<string>> GetDetailsAll();

        //get district
        Task<List<string>> GetDistrictAll();

        //get all price
        Task<List<string>> GetAllPrice();








    }
}
