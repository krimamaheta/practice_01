using Microsoft.AspNetCore.Mvc;
using Practice_01.Models;
using Practice_01.ViewModel;

namespace Practice_01.Repository
{
    public interface IImageRepository
    {
        //Task<string> UploadImageAsync(ImageModel imageModel);
        Task<List<string>> UploadImageAsync([FromForm] List<IFormFile>files, [FromForm] Guid? vendorEventId);
        Task<IEnumerable<ImageModel>> GetAllImagesAsync();



        //event image
        //Task<VendorEventModel> AddEventImage(VendorEventModel Model);
    }
}
