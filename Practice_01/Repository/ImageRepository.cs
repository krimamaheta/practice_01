using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_01.Data;
using Practice_01.Models;
using Practice_01.ViewModel;

namespace Practice_01.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;
        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<ImageModel>> GetAllImagesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> UploadImageAsync([FromForm] List<IFormFile> files, [FromForm] Guid? vendorEventId)
        {
            throw new NotImplementedException();
        }
        //public async Task<List<string>> UploadImageAsync([FromForm] List<IFormFile> files, [FromForm] Guid? vendorEventId)
        //{

        //    try
        //    {
        //        var imagePaths=new List<string>();
        //        foreach (var file in files)
        //        {
        //            if (file == null || file.Length == 0)
        //                throw new ArgumentException("No file uploaded");

        //            // Save image to disk or database
        //            var imagePath = await SaveImageAsync(file);

        //            var image = new Image
        //            {
        //                ImageUrl = imagePath,
        //                VendorEventId = vendorEventId.GetValueOrDefault(),
        //               // VendorEventId = (Guid)vendorEventId,
        //            };
        //            _context.Images.Add(image);
        //            await _context.SaveChangesAsync();

        //            imagePaths.Add(imagePath);
        //        }

        //        return imagePaths;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //try
        //{
        //    var file = imageModel.FormFile;
        //    if (file == null || file.Length == 0)
        //        throw new ArgumentException("No file uploaded");

        //    // Save image to disk or database
        //    var imagePath = await SaveImageAsync(file);

        //    var image = new Image
        //    {
        //        ImageUrl = imagePath,
        //        //Prices = imageModel.Prices,
        //        VendorEventId = (Guid)imageModel.VendorEventId,
        //    };
        //    _context.Images.Add(image);
        //    await _context.SaveChangesAsync();
        //    // Process other data as needed

        //    return imagePath;
        //}
        //catch (Exception ex)
        //{
        //    // Handle exception
        //    throw ex;
        //}
    }

        //private async Task<string> SaveImageAsync(IFormFile file)
        //{
        //    var filePath = Path.GetTempFileName(); // Change this to your desired file path
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    return filePath;
        //}
        //private readonly List<ImageModel> _images = new List<ImageModel>();

        //public async Task<IEnumerable<ImageModel>> GetAllImagesAsync()
        //{
        //    // In a real scenario, you would retrieve images from a database or another data source.
        //    // Here, we are just returning the list of images.

        //    var images = await _context.Images.ToListAsync();
        //    return images.Select(im => new ImageModel
        //    {
        //        // Set other properties as needed
        //        VendorEventId = im.VendorEventId
        //    });
        //}
        //add event image


       
    }

