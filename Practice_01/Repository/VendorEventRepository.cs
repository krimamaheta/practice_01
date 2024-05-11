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
using Stripe;

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
                var data = await _context.VendorEvents
             .Include(x => x.Vendor)
             .Include(y => y.Events)
             .FirstOrDefaultAsync(vendorEvent => vendorEvent.Id == Id);

                if (data == null)
                {
                    Console.WriteLine($"No data found for ID {Id}");
                    return null;
                }

                var vendorEventModel = new VendorEventModel
                {
                    Id = data.Id,
                    VendorId = data.VendorId,
                    EventId = data.EventId,
                    DishName = data.DishName,
                    Price = data.Price,
                    //Images = data.Images.Select(x => x.ImageUrl).ToList()
                    // Add other properties as needed
                };

                return vendorEventModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving data for  ID {Id}: {ex.Message}");
                throw;
            }
            //try
            //{
            //    var eventvendor = await _context.VendorEvents.
            //        Include(x => x.Vendor)
            //        .Include(y => y.Events)
            //        .FirstOrDefaultAsync(vendorevent => vendorevent.Id == Id);
            //    if (eventvendor == null|| eventvendor.Vendor == null || eventvendor.Events == null)
            //    {
            //        return null;
            //    }

            //    //map
            //    var vendoreventmodel = new VendorEventModel
            //    {
            //        Id = eventvendor.Id,
            //        Price = eventvendor.Price,
            //        Images = eventvendor.Images.Select(x => x.ImageUrl).ToList(),
            //        FirmName = eventvendor.Vendor.FirmName,
            //        CityName = eventvendor.Vendor.CityName,
            //        EventName = eventvendor.Events.EventName,
            //        Address = eventvendor.Vendor.Address,
            //        WebsiteUrl = eventvendor.Vendor.WebsiteUrl,
            //        District = eventvendor.Vendor.District,

            //    };
            //    return vendoreventmodel;

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("$\"An error occurred while retrieving VendorEvent: {ex.Message}\"");
            //    throw;
            //}
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

        public async Task<List<SearchModel>> GetDecorationAsync(SearchModel model)
        
        
        {
            try
            {
                //var decorations = await _context.VendorEvents
                //                .Include(x => x.Vendor)
                //                .Include(x => x.Events)
                //                .Where(d => d.District == vendorEvent.District &&
                //                    d.CityName == vendorEvent.CityName &&
                //                    d.Price == vendorEvent.Price &&
                //                    d.EventId == vendorEvent.EventId)
                //                .Select(d => new VendorEventModel
                //                {
                //                    EventName = vendorEvent.EventName,
                //                    Images = vendorEvent.Images,
                //                    Price = vendorEvent.Price,
                //                }).ToListAsync();

                //return decorations;
                var query =_context.VendorEvents
                        .Include(x => x.Vendor)
                        .Include(y => y.Events)
                        .AsQueryable();
                if (!string.IsNullOrEmpty(model.District) || (model.District != null && model.District.Any()))
                {
                    query=query.Where(v=>v.Vendor.District==model.District);

                }
                if(!string.IsNullOrEmpty(model.CityName) || (model.CityName != null && model.CityName.Any()))
                {
                    query = query.Where(v => v.Vendor.CityName == model.CityName);
                }
                if(!string .IsNullOrEmpty(model.Price) || (model.Price!=null && model.Price.Any()))
                {
                    //query = query.Where(v => v.Price.ToString() == model.Price);
                    decimal price = decimal.Parse(model.Price);
                    query = query.Where(v => v.Price == price);
                }
                if(!string.IsNullOrEmpty(model.EventId) || (model.EventId!=null &&model.EventId.Any()))
                {
                    //query = query.Where(ve => ve.Events.Id.ToString() == model.EventId);
                    Guid eventId = Guid.Parse(model.EventId);
                    query = query.Where(ve => ve.Events.Id == eventId);
                }
                var decoration = await query
                    .Select(v => new SearchModel
                    {
                        EventId=v.Events.Id.ToString(),
                        ImageUrls = v.Images.Select(i => i.ImageUrl).ToList(),
                        EventName = v.Events.EventName,
                        Price = v.Price.ToString(),
                        CityName=v.Vendor.CityName,
                        District=v.Vendor.District,

                    }).ToListAsync();

                return decoration;

            }
            catch(Exception ex)
            {
                Console.WriteLine("fail to fetch list of values");
                return new List<SearchModel>();
            }
        }
        //get all caterer 
        public async Task<List<VendorEventModel>> GetAllCaterer()
        {
           
            
            var caterer=await _context.VendorEvents
                .Include(x=>x.Vendor)
                .Where(y=>y.Vendor.TypeOfVendor=="Caterer")
                .Select(ye=>new VendorEventModel
                {
                    Id = ye.Id,
                    VendorId =ye.VendorId,
                    EventId=ye.EventId,
                    DishName=ye.DishName,
                    Price=ye.Price,
                    Images=ye.Images.Select(i=>i.ImageUrl).ToList(),
                }).ToListAsync();
            return caterer;

            
          
        }


        //get all caterer by vendorid
        public async Task<List<VendorEventModel>> GetAllCatererById(Guid vendorId)
        {
            try
            {
                var data = await _context.VendorEvents
                    .Include(x => x.Vendor)
                    .Include(y => y.Events)
                    .Where(vendorevent => vendorevent.VendorId == vendorId)
                    .Select(vendorevent => new VendorEventModel
                    {
                        
                        VendorId = vendorevent.VendorId,
                        DishName=vendorevent.DishName,
                        Price = vendorevent.Price,
                        Images=vendorevent.Images.Select(x=>x.ImageUrl).ToList(),
                    }).ToListAsync();
                Console.WriteLine($"Retrieved {data.Count} records for vendor with ID {vendorId}");
             
                return data;
            }catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving data for vendor with ID {vendorId}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<VendorEventModel>>GetDishNameWithPrice(Guid Id)
        {
            try
            {
                var data =_context.VendorEvents
                .Include(x => x.Vendor)
                .Include(y => y.Events)
                .Where(x => x.Id == Id)
                .Select(vendorEvent => new VendorEventModel
                {
                    Id = vendorEvent.Id,
                    DishName = vendorEvent.DishName,
                    Price = vendorEvent.Price
                }).ToList();
                Console.WriteLine("retrival fail");
                return data;
            }catch(Exception ex )
            {
                Console.WriteLine($"An error occurred while retrieving data for  ID {Id}: {ex.Message}");
                throw;
            }
        }

        public Task<bool> AddVendorEvent(VendorEventModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddImage(ImageModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}








