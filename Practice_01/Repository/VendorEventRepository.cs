using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Practice_01.Data;
using Practice_01.Models;
using Practice_01.ViewModel;
using CloudinaryDotNet;
using Azure.Messaging;
using System.Linq;

namespace Practice_01.Repository
{
    public class VendorEventRepository : IVendorEventRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Cloudinary _cloudinary;
        public VendorEventRepository(ApplicationDbContext context, IImageRepository imageRepository, Cloudinary cloudinary, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _imageRepository = imageRepository;
            _webHostEnvironment = webHostEnvironment;
            _cloudinary = cloudinary;
        }

        //public async Task<VendorEventModel> AddEventvendorDetails(VendorEventModel Model)
        //{
        //    var vendorevent = new VendorEvent
        //    {
        //        Id = Guid.NewGuid(),
        //        VendorId = Model.VendorId,
        //        EventId = Model.EventId,
        //        Price = Model.Price
        //    };
        //    if (Model.Images.Any() && Model.Images != null)
        //    {
        //        vendorevent.Images = Model.Images.Select(img => new Image { ImageUrl = img }).ToList();
        //    }
        //    _context.VendorEvents.Add(vendorevent);
        //    await _context.SaveChangesAsync();
        //    return new VendorEventModel
        //    {
        //        VendorId = Model.VendorId,
        //        EventId = Model.EventId,
        //        Price = Model.Price,
        //        //Images = vendorevent.Images.Select(img => img.ImageUrl)
        //    };
        //}

        //try
        //{
        //    var newEvent = new VendorEvent
        //    {
        //        EventId = Model.EventId,
        //        VendorId = Model.VendorId,
        //        Price = Model.Price,
        //        //Images = images
        //    };

        //    var imagePaths = await _imageRepository.UploadImageAsync(files,vendorEventId);

        //    var images = imagePaths.Select(imagePath => new Image
        //    {
        //        ImageUrl =imagePath,
        //        VendorEventId =(Guid)vendorEventId
        //    }).ToList();


        //    _context.VendorEvents.Add(newEvent);
        //    await _context.SaveChangesAsync();

        //    return Model;
        //}
        //    catch (Exception ex)
        //{
        //    throw ex;
        //}




        //public async Task<bool> AddImage(ImageModel viewModel)
        //{
        //    try
        //    {
        //        // Map ViewModel to Model
        //        var image = new Image
        //        {
        //            Id = Guid.NewGuid(), // Generate new Id

        //            // ImageUrl = viewModel.FormFile.ToString(),
        //            ImageUrl = viewModel.ImageUrl.ToString(),
        //            //Prices=viewModel.Prices,
        //            VendorEventId = viewModel.VendorEventId.GetValueOrDefault() // Assuming VendorEventId is provided
        //        };

        //        // Add Image to database
        //        _context.Images.Add(image);
        //        await _context.SaveChangesAsync();

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        // Log or handle the exception
        //        return false;
        //    }
        //}

        public async Task<bool> AddEventvendorDetails(VendorEventModel viewModel, Cloudinary cloudinary)
        {

            try
            {
                var vendorEvent = new VendorEvent
                {
                    VendorId = viewModel.VendorId,
                    EventId = viewModel.EventId,
                    Price = viewModel.Price,
                    DishName = viewModel.DishName,
                };

                // Add the created VendorEvent instance to the context and save changes
                _context.VendorEvents.Add(vendorEvent);
                await _context.SaveChangesAsync();
                if (viewModel.Images != null) // Check if viewModel.Images is not null
                {
                    foreach (var imageUrl in viewModel.Images)
                    {
                        try
                        {
                            // Validate the image URL
                            if (Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri? uri))
                            {
                                var uploadParams = new ImageUploadParams
                                {
                                    File = new FileDescription(imageUrl) // No need for FileName or OpenReadStream when using URL
                                };

                                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                                var uploadedImageUrl = uploadResult.SecureUri.AbsoluteUri;

                                var image = new Image
                                {
                                    Id = Guid.NewGuid(),
                                    ImageUrl = uploadedImageUrl,
                                    VendorEventId = vendorEvent.Id
                                };

                                _context.Images.Add(image);
                            }
                            else
                            {
                                throw new ArgumentException($"Invalid image URL: {imageUrl}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while processing image: {ex.Message}");
                        }
                    }
                }

                await _context.SaveChangesAsync();

                // Return true if the operation is successful
                return true;

                //foreach (var imageUrl in viewModel.Images)
                //{
                //    try
                //    {
                //        // Validate the image URL
                //        if (Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri? uri))
                //        {
                //            var uploadParams = new ImageUploadParams
                //            {
                //                File = new FileDescription(imageUrl) // No need for FileName or OpenReadStream when using URL
                //            };

                //            var uploadResult = await cloudinary.UploadAsync(uploadParams);

                //            var uploadedImageUrl = uploadResult.SecureUri.AbsoluteUri;

                //            var image = new Image
                //            {
                //                Id = Guid.NewGuid(),
                //                ImageUrl = uploadedImageUrl,
                //                VendorEventId = vendorEvent.Id
                //            };

                //            _context.Images.Add(image);
                //        }
                //        else
                //        {
                //            throw new ArgumentException($"Invalid image URL: {imageUrl}");
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine($"An error occurred while processing image: {ex.Message}");
                //    }
                //}

                //await _context.SaveChangesAsync();

                //// Return true if the operation is successful
                //return true;

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"An error occurred while adding vendor event details: {ex.Message}");
                return false;
            }


            //try
            //{
            //    var vendorEvent = new VendorEvent
            //    {
            //        VendorId = viewModel.VendorId,
            //        EventId = viewModel.EventId,
            //        Price = viewModel.Price,
            //        DishName = viewModel.DishName,
            //    };

            //    // Add the created VendorEvent instance to the context and save changes
            //    _context.VendorEvents.Add(vendorEvent);
            //    await _context.SaveChangesAsync(); // Save changes to generate ID

            //    // Now that the vendorEvent has been saved to the database, it has an ID
            //    foreach (var imageUrl in viewModel.Images)
            //    {
            //        try
            //        {
            //            // Validate the image URL
            //            if (Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri? uri))
            //            {
            //                var uploadParams = new ImageUploadParams
            //                {
            //                    File = new FileDescription(imageUrl) // No need for FileName or OpenReadStream when using URL
            //                };

            //                var uploadResult = await cloudinary.UploadAsync(uploadParams);

            //                var uploadedImageUrl = uploadResult.SecureUri.AbsoluteUri;

            //                var image = new Image
            //                {
            //                    Id = Guid.NewGuid(),
            //                    ImageUrl = uploadedImageUrl,
            //                    VendorEventId = vendorEvent.Id // Now vendorEvent.Id should not be null
            //                };

            //                _context.Images.Add(image);
            //            }
            //            else
            //            {
            //                throw new ArgumentException($"Invalid image URL: {imageUrl}");
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine($"An error occurred while processing image: {ex.Message}");
            //        }
            //    }

            //    // Save changes to add images to the database
            //    await _context.SaveChangesAsync();

            //    // Return true if the operation is successful
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error while adding vendor event: {ex.Message}");
            //    // Return false or handle the error as appropriate
            //    return false;
            //}
        }

        public IQueryable<ImageModel> GetAllImagesWithPrices()
        {
            return _context.Images
                .Select(i => new ImageModel
                {
                    //ImageUrl = i.ImageUrl,
                    // Prices = i.Prices,
                    VendorEventId = i.VendorEventId
                });
        }


        //list of images
        public async Task<List<VendorEventModel>> GetAllEventvendorDetails()
        {
            try
            {
                var vendorevents = await _context.VendorEvents
                    .Include(ve => ve.Images)
                    .Select(ve => new VendorEventModel
                    {
                        Id = ve.Id,
                        VendorId = ve.VendorId,
                        EventId = ve.EventId,
                        Price = ve.Price,

                        // Images=ve.Images.Select(i=>i.ImageUrl).ToList(),
                    })
                    .ToListAsync();

                return vendorevents;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving vendor events: {ex.Message}");
                return null;
            }
        }
        public Task<bool> AddVendorEvent(VendorEventModel viewModel)
        {
            throw new NotImplementedException();
        }


        //get all with listImages
        public List<VendorEventModel> GetAllVendorEventImages()
        {

            try
            {
                var data = _context.VendorEvents
                    .Include(x => x.Vendor)
                    .Include(y => y.Events)
                    .Select((vendorEvent) => new VendorEventModel
                    {
                        Price = vendorEvent.Price,
                        Images = vendorEvent.Images.Select(img => img.ImageUrl).ToList(),
                        FirmName = vendorEvent.Vendor.FirmName,
                        EventName = vendorEvent.Events.EventName,
                        CityName = vendorEvent.Vendor.CityName,
                        Id = vendorEvent.Id,
                    }).ToList();

                // Log the count of retrieved data
                Console.WriteLine($"Retrieved {data.Count} records");



                // Return the retrieved data
                return data;
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during data retrieval
                Console.WriteLine($"An error occurred while retrieving data: {ex.Message}");
                // Optionally, rethrow the exception to propagate it up the call stack
                throw;
            }
        }

        public Task<bool> AddImage(ImageModel viewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteVendorById(Guid EventVendorId)
        {
            try
            {
                var existID = await _context.VendorEvents.FindAsync(EventVendorId);
                if (existID == null)
                {
                    return false;
                }
                _context.VendorEvents.Remove(existID);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("$\"An error occurred while deleting VendorEvent: {ex.Message}\"");
                throw;
            }
        }

        public async Task<VendorEventModel> GetvendorEventById(Guid Id)
        {
            try
            {
                var eventvendor = await _context.VendorEvents.
                    Include(x => x.Vendor)
                    .Include(y => y.Events)
                    .FirstOrDefaultAsync(vendorevent => vendorevent.Id == Id);
                if (eventvendor == null)
                {
                    return null;
                }
                if (eventvendor.Vendor == null || eventvendor.Events == null)
                {
                    // Handle the case where navigation properties are null
                    // For example, you could return null or throw an exception
                    return null;
                }
                //map
                var vendoreventmodel = new VendorEventModel
                {
                    Price = eventvendor.Price,
                    Images = eventvendor.Images.Select(x => x.ImageUrl).ToList(),
                    FirmName = eventvendor.Vendor.FirmName,
                    CityName = eventvendor.Vendor.CityName,
                    EventName = eventvendor.Events.EventName,
                    Address = eventvendor.Vendor.Address,
                    WebsiteUrl = eventvendor.Vendor.WebsiteUrl,
                    District = eventvendor.Vendor.District,
                    Id = eventvendor.Id
                };
                return vendoreventmodel;

            }
            catch (Exception ex)
            {
                Console.WriteLine("$\"An error occurred while retrieving VendorEvent: {ex.Message}\"");
                throw;
            }
        }

        public async Task<List<VendorEventModel>> GetAllVendorId(Guid vendorId)
        {
            try
            {
                var data = await _context.VendorEvents
                    .Include(x => x.Vendor)
                    .Include(y => y.Events)
                    .Select((vendorevent) => new VendorEventModel
                    {
                        Price = vendorevent.Price,
                        Images = vendorevent.Images.Select(x => x.ImageUrl).ToList(),
                        FirmName = vendorevent.Vendor.FirmName,
                        EventName = vendorevent.Events.EventName,
                        CityName = vendorevent.Vendor.CityName,
                        Address = vendorevent.Vendor.Address,
                        District = vendorevent.Vendor.District,
                        WebsiteUrl = vendorevent.Vendor.WebsiteUrl,
                        DishName = vendorevent.DishName,
                        Id = vendorevent.Id,
                        VendorId = vendorevent.VendorId
                    }).Where(x => x.VendorId == vendorId).ToListAsync();
                Console.WriteLine($"Retrieved {data.Count} records");
                Console.WriteLine("get all base on vendorid");

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving data: {ex.Message}");
                // Optionally, rethrow the exception to propagate it up the call stack
                throw;
            }
        }

        //update all
        public async Task<VendorEvent> UpdateVendorEvent(Guid vendorId, VendorEventModel model)
        {
            // Find the vendor event with the specified vendorId
            var vendorEvent = await _context.VendorEvents.FirstOrDefaultAsync(x => x.VendorId == vendorId);

            if (vendorEvent == null)
            {
                // No vendor event found for the given vendorId
                return null;
            }

            // Update specific details of the vendor event
            vendorEvent.Price = model.Price;
            vendorEvent.DishName = model.DishName;

            // Update images if provided
            //if (model.Images != null && model.Images.Any())
            //{
            //    foreach (var imageUrl in model.Images)
            //    {
            //        try
            //        {
            //            // Validate the image URL
            //            if (Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri? uri))
            //            {
            //                var uploadParams = new ImageUploadParams
            //                {
            //                    File = new FileDescription(imageUrl) // No need for FileName or OpenReadStream when using URL
            //                };

            //                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            //                var uploadedImageUrl = uploadResult.SecureUri.AbsoluteUri;

            //                // Create a new Image entity and associate it with the vendor event
            //                var newImage = new Image
            //                {
            //                    Id = Guid.NewGuid(),
            //                    ImageUrl = uploadedImageUrl,
            //                    VendorEventId = vendorEvent.Id
            //                };

            //                // Add the new image to the collection of images associated with the vendor event
            //                vendorEvent.Images.Add(newImage);
            //            }
            //            else
            //            {
            //                throw new ArgumentException($"Invalid image URL: {imageUrl}");
            //            }
            if (model.Images != null && model.Images.Any())
            {
                foreach (var imageUrl in model.Images)
                {
                    try
                    {
                        // Validate the image URL
                        if (Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri? uri))
                        {
                            var uploadParams = new ImageUploadParams
                            {
                                File = new FileDescription(imageUrl) // No need for FileName or OpenReadStream when using URL
                            };

                            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                            var uploadedImageUrl = uploadResult.SecureUri.AbsoluteUri;

                            // Create a new Image entity and associate it with the vendor event
                            var newImage = new Image
                            {
                                Id = Guid.NewGuid(),
                                ImageUrl = uploadedImageUrl,
                                VendorEventId = vendorEvent.Id,
                                VendorEvent = vendorEvent // Set the reference to the VendorEvent
                            };

                            // Add the new image to the collection of images associated with the vendor event
                            vendorEvent.Images.Add(newImage);
                        }
                        else
                        {
                            throw new ArgumentException($"Invalid image URL: {imageUrl}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while processing image: {ex.Message}");
                    }
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return vendorEvent;
        }

        public async Task<List<string>> GetDetailsAll()
        {
            var uniquecityname = _context.Vendors
                  .Select(x => x.CityName)
                  .Where(cityname => !string.IsNullOrEmpty(cityname))
                  .Distinct()
                  
                  .ToList();
            return uniquecityname;

            //var data = await _context.VendorEvents
            //     .Include(x => x.Vendor)
            //     .Include(y => y.Events)
            //     .Select(vendorevent => new VendorEvent // Projecting to VendorEvent instead of VendorEventModel
            //     {
            //         Price = vendorevent.Price,
            //         CityName = vendorevent.Vendor.CityName,
            //         District = vendorevent.Vendor.District,
            //         // Include other properties of VendorEvent as needed
            //     }).ToListAsync();

            //return data;
        }

        public async Task<List<string>> GetDistrictAll()
        {
            var res=_context.Vendors
                .Select(x=>x.District)
                .Where(district=>!string.IsNullOrEmpty(district))
                .Distinct()
                .ToList();

            return res;
        }


        public async Task<List<string>> GetAllPrice()
        {

            var res =await _context.Vendors
                .Where(x => x.TypeOfVendor == "Decorator")
                .SelectMany(v => v.VendorEvents)
                .Select(x => x.Price.ToString())
                .Distinct()
                .ToListAsync();
            return res;

         //   var res = _context.VendorEvents
         //.Select(x => x.Price)
         //.Where(price => price.HasValue) // Filter out null values
         //.Select(price => price.Value.ToString()) // Convert decimal? to string
         //.Distinct()
         //.ToList();
         //   return res;
        }
    }
}




            //try
            //{
            //    var vendorevents = await _context.VendorEvents
            //                        .Include(x => x.Vendor)
            //                        .Include(x => x.Events)
            //                        .Where(y => y.VendorId == vendorId)
            //                        .ToListAsync();
            //    foreach(var vendorEvent in vendorevents)
            //    {
            //        //vendorEvent.VendorId = vendorId;

//       if(vendorEvent.Vendor!=null)
//        {
//        vendorEvent.Vendor.FirmName = model.FirmName;
//        vendorEvent.Vendor.CityName = model.CityName;
//        vendorEvent.Vendor.Address=model.Address;
//        vendorEvent.Vendor.District = model.District;
//        vendorEvent.Vendor.WebsiteUrl = model.WebsiteUrl;

//        }
//        if (vendorEvent.Events != null)
//        {
//        vendorEvent.Events.EventName=model.EventName;

//        }

//        if (model.Images != null)
//        {
//            vendorEvent.Images = model.Images.Select(url => new Image { ImageUrl = url }).ToList();
//        }
//        vendorEvent.Price = model.Price;

//        _context.VendorEvents.Update(vendorEvent);
//        await _context.SaveChangesAsync();  
//        //vendorEvent.Vendor.
//    }


//}catch(Exception ex)
//{
//    Console.WriteLine($"An error occurred while updating vendor events: {ex.Message}");
//    throw;
//}







//public static IFormFile ConvertToIFormFile(Practice_01.Models.Image image)
//{
//    if (image == null)
//    {
//        throw new ArgumentNullException(nameof(image), "Image is null.");
//    }

//    if (string.IsNullOrWhiteSpace(image.ImageUrl))
//    {
//        throw new ArgumentException("Image URL is null or empty.", nameof(image));
//    }

//    // Validate image URL
//    if (!Uri.TryCreate(image.ImageUrl, UriKind.Absolute, out var uri) ||
//        !uri.Scheme.StartsWith("http", StringComparison.OrdinalIgnoreCase) &&
//        !uri.Scheme.StartsWith("https", StringComparison.OrdinalIgnoreCase))
//    {
//        throw new ArgumentException("Invalid image URL format.");
//    }

//    // Create an HttpClient instance to download the image content
//    using (var httpClient = new HttpClient())
//    {
//        try
//        {
//            // Download the image content as a byte array
//            byte[] imageData = httpClient.GetByteArrayAsync(uri).Result;

//            // Create a MemoryStream from the image data
//            using (var memoryStream = new MemoryStream(imageData))
//            {
//                // Create an IFormFile instance from the MemoryStream
//                return new FormFile(memoryStream, 0, memoryStream.Length, "file", Path.GetFileName(uri.LocalPath));
//            }
//        }
//        catch (Exception ex)
//        {
//            throw new Exception($"Error converting image to IFormFile: {ex.Message}", ex);
//        }
//    }
//}

//        public List<VendorEventModel> GetAllVendorEventImages()
//        {
//        var query = from vendorEvent in _context.VendorEvents
//                    join vendor in _context.Vendors on vendorEvent.VendorId equals vendor.Id
//                    join events in _context.Events on vendorEvent.EventId equals events.Id
//                    where vendor.TypeOfVendor == "Decorator"
//                    select new VendorEventModel
//                    {
//                        //VendorId = vendorEvent.VendorId,
//                        // EventId = vendorEvent.EventId,
//                        // Price = vendorEvent.Price,
//                        //Images = (List<IFormFile>)vendorEvent.Images,
//                        Images = vendorEvent.Images.Select(imageUrl => ConvertToIFormFile(imageUrl)).ToList()
//                    };
//        //Images = vendorEvent.Images.Select(img => new FormFile(new MemoryStream(), 0, 0, "file", img.ImageUrl) as IFormFile).ToList()
//            };
//            return query.ToList();
//        }
//private IFormFile ConvertToIFormFile(string imageUrl)
//{
//    using (var httpClient = new HttpClient())
//    {
//        var imageContent = httpClient.GetByteArrayAsync(imageUrl).Result;
//        var fileName = Path.GetFileName(imageUrl);
//        return new FormFile(new MemoryStream(imageContent), 0, imageContent.Length, "file", fileName);
//    }
//}



//public IQueryable<VendorEvent> GetVendorEvents(string userSelectedCity, string userSelectedEventName, decimal userGivenPrice)
//{
//    if (!Enum.TryParse(userSelectedEventName, out EventName selectedEvent))
//    {
//        // Handle invalid event name
//        throw new ArgumentException("Invalid event name.");
//    }
//    return _context.VendorEvents
//    .Include(ve => ve.Vendor)
//    .ThenInclude(v => v.User)
//    .Include(ve => ve.Events)
//    .Include(ve=>ve.Images)
//    .Where(ve => ve.Vendor.CityName == userSelectedCity)
//    .Where(ve => ve.Events.EventName == selectedEvent)
//    .Where(ve => ve.Price == userGivenPrice);
//}




//public IQueryable<VendorEvent> GetVendorEvents(string userSelectedCity, EventName userSelectedEvent, decimal userGivenPrice)
//{
//    return _context.VendorEvents
//        .Include(ve => ve.Vendor)
//        .ThenInclude(v => v.User)
//        .Include(ve => ve.Events)
//        .Where(ve => ve.Vendor.CityName == userSelectedCity)
//        .Where(ve => ve.Events.EventName == userSelectedEvent)
//        .Where(ve => ve.Price == userGivenPrice);
//}






