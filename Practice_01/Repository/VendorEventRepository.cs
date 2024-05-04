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

namespace Practice_01.Repository
{
    public class VendorEventRepository : IVendorEventRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VendorEventRepository(ApplicationDbContext context, IImageRepository imageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _imageRepository = imageRepository;
            _webHostEnvironment = webHostEnvironment;
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
            //try
            //{
            //    var vendorEvent = new VendorEvent
            //    {
            //        VendorId = viewModel.VendorId,
            //        EventId = viewModel.EventId,
            //        Price = viewModel.Price
            //    };

            //    // Add the created VendorEvent instance to the context and save changes
            //    _context.VendorEvents.Add(vendorEvent);
            //    await _context.SaveChangesAsync();

                //foreach (var formFile in viewModel.Images)
                //{
                //    if (formFile != null && formFile.Length > 0)
                //    {
                //        // Upload image to Cloudinary
                //        var uploadParams = new ImageUploadParams
                //        {
                //            File = new FileDescription(formFile.FileName, formFile.OpenReadStream())

                //        };

                //        var uploadResult = await cloudinary.UploadAsync(uploadParams);

                //        var imageUrl = uploadResult.SecureUri.AbsoluteUri;

                //        var image = new Image
                //        {
                //            Id = Guid.NewGuid(),
                //            ImageUrl = imageUrl,
                //            VendorEventId = vendorEvent.Id
                //        };

                //        _context.Images.Add(image);
                //    }
                //}
                // await _context.SaveChangesAsync();

                //return true;


                try
                {
                    var vendorEvent = new VendorEvent
                    {
                        VendorId = viewModel.VendorId,
                        EventId = viewModel.EventId,
                        Price = viewModel.Price
                    };

                    // Add the created VendorEvent instance to the context and save changes
                    _context.VendorEvents.Add(vendorEvent);
                    await _context.SaveChangesAsync();

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

                    await _context.SaveChangesAsync();

                    // Return true if the operation is successful
                    return true;



                    // Add images
                    //foreach (var formFile in viewModel.Images)
                    //{
                    //    if (formFile != null && formFile.Length > 0)
                    //    {
                    //        // Generate unique filename
                    //        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                    //        // Get the path to the "wwwroot" folder
                    //        var webRootPath = _webHostEnvironment.ContentRootPath; // This should give you the root path of the web application

                    //        // Check if webRootPath is not null
                    //        if (webRootPath == null)
                    //        {
                    //            throw new Exception("Web root path is null.");
                    //        }

                    //        // Combine file path
                    //        var uploadsPath = Path.Combine(webRootPath, "uploads");

                    //        // Check if uploadsPath exists, if not, create it
                    //        if (!Directory.Exists(uploadsPath))
                    //        {
                    //            Directory.CreateDirectory(uploadsPath);
                    //        }

                    //        var filePath = Path.Combine(uploadsPath, fileName);

                    //        // Save image to file system
                    //        using (var stream = new FileStream(filePath, FileMode.Create))
                    //        {
                    //            await formFile.CopyToAsync(stream);
                    //        }

                    //        var imageUrl = "/uploads/" + fileName; // URL to access the uploaded image

                    //        var image = new Image
                    //        {
                    //            Id = Guid.NewGuid(),
                    //            ImageUrl = imageUrl,
                    //            VendorEventId = vendorEvent.Id
                    //        };

                    //        _context.Images.Add(image);
                    //    }
                    //}

                    //// Add images
                    ////foreach (var imageUrl in viewModel.Images)
                    ////{
                    ////    var image = new Image
                    ////    {
                    ////        Id = Guid.NewGuid(), // Ensure to set a new GUID for each image
                    ////        ImageUrl = imageUrl.ToString(),
                    ////        VendorEventId = vendorEvent.Id
                    ////    };
                    ////    _context.Images.Add(image);
                    ////}



                    //// Save changes to add images

                }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"An error occurred while adding vendor event details: {ex.Message}");
                return false;
            }
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
            //var query = from vendorEvent in _context.VendorEvents
            //            join vendor in _context.Vendors on vendorEvent.VendorId equals vendor.Id
            //            join events in _context.Events on vendorEvent.EventId equals events.Id
            //            where vendor.TypeOfVendor == "Decorator"

            //            select new VendorEventModel
            //            {
            //                //Id = vendorEvent.Id,
            //               // VendorId = vendorEvent.VendorId,
            //               // EventId = vendorEvent.EventId,
            //                Price = vendorEvent.Price,
            //                Images=vendorEvent.Images.Select(img=>img.ImageUrl).ToList(),
            //                FirmName=vendor.FirmName,
            //                cityName=vendor.CityName,

            //                //Images=vendorEvent.Images.Select(img=> ConvertToIFormFile(img)).ToList()
            //               // Images = vendorEvent.Images.Select(img => ConvertToIFormFile(img.ImageUrl)).ToList()
            //            };
            //return query.ToList();

            //var data = _context.VendorEvents.Include(x => x.Vendor).Include(y => y.Events).Select((vendorEvent) =>
            // new VendorEventModel
            // {
            //     Price = vendorEvent.Price,
            //     Images = vendorEvent.Images.Select(img => img.ImageUrl).ToList(),
            //     FirmName = vendorEvent.Vendor.FirmName,
            //     EventName = vendorEvent.Events.EventName,
            //     CityName = vendorEvent.Vendor.CityName,
            // }).ToList();
            //return new List<VendorEventModel>();
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
                        Id= vendorEvent.Id,
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
                if(existID == null)
                {
                    return false;
                }
                _context.VendorEvents.Remove(existID);
                await _context.SaveChangesAsync();
                return true;

            }catch(Exception ex) {
                Console.WriteLine("$\"An error occurred while deleting VendorEvent: {ex.Message}\"");
                throw;
            }
        }

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
    }



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


   

}

