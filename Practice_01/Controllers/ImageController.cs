using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Practice_01.Repository;
using Practice_01.ViewModel;

namespace Practice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IEventRepository _eventRepository;
        public ImageController(IImageRepository imageRepository, IEventRepository eventRepository)
        {
            _imageRepository = imageRepository;
            _eventRepository = eventRepository;
        }
        //[HttpPost("UploadImages")]
        //public async Task<IActionResult>UploadImage(ImageModel imageModel)
        //{
        //    try
        //    {
        //        var imagePaths = await _imageRepository.UploadImageAsync(imageModel.FormFile, imageModel.VendorEventId);
        //        return Ok(imagePaths);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpGet("AllImages")]
        public async Task<IActionResult>GetAllImages()
        {
            try
            {
                var image = await _imageRepository.GetAllImagesAsync();
                return Ok(image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //add decorator event image
        //[HttpPost("AddEventWithImage")]
        //public async Task<IActionResult>AddEventWithImage(EventImageModel Model)
        //{

        //    //try
        //    //{
        //    //    var addEvent = await _eventRepository.AddEvent(Model.EventModel);
        //    //    Model.VendorEventId = (Guid)addEvent.EventId;

        //    //    //add image
        //    //    var addImage=await _imageRepository.AddEventImage(Model);

        //    //    return Ok(new
        //    //    {
        //    //        Event = addEvent,
        //    //        Image = addImage
        //    //    });
        //    //}
        //    //catch(Exception ex)
        //    //{
        //    //    return StatusCode(500, $"Internal server error: {ex.Message}");
        //    //}

        //}
    }
}
