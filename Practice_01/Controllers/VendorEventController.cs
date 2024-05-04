﻿using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Practice_01.Models;
using Practice_01.Repository;
using Practice_01.ViewModel;
using practice1.Services;

namespace Practice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorEventController : ControllerBase
    {
        private readonly IVendorEventRepository _vendorEventRepository;
        private readonly Cloudinary _cloudinary;
        public VendorEventController(IVendorEventRepository vendorEventRepository, Cloudinary cloudinary)
        {
            _vendorEventRepository = vendorEventRepository;
            _cloudinary = cloudinary;

        }

        [HttpPost("AddVendorEventDetails")]
        public async Task<IActionResult> AddVendorEventDetails(VendorEventModel viewModel)
        {
            try
            {
                // Call the repository method to add the VendorEvent
                var result = await _vendorEventRepository.AddVendorEvent(viewModel);

                // Check if the operation was successful
                if (result)
                {
                    // Return success response
                    return Ok(new { Success = true, Message = "VendorEvent added successfully." });
                }
                else
                {
                    // Return error response if the operation failed
                    return BadRequest(new { Success = false, Message = "Failed to add VendorEvent." });
                }
            }
            catch (Exception ex)
            {
                // Return error response if an exception occurs
                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }


        [HttpPost("AddImage")]
        public async Task<IActionResult> AddImage(ImageModel viewModel)
        {
            try
            {
                // Call the repository method to add the Image
                var result = await _vendorEventRepository.AddImage(viewModel);

                // Check if the operation was successful
                if (result)
                {
                    // Return success response
                    return Ok(new { Success = true, Message = "Image added successfully." });
                }
                else
                {
                    // Return error response if the operation failed
                    return BadRequest(new { Success = false, Message = "Failed to add Image." });
                }
            }
            catch (Exception ex)
            {
                // Return error response if an exception occurs
                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet("GetAllImagesWithPrices")]
        public IActionResult GetAllImagesWithPrices()
        {
            var imagesWithPrices = _vendorEventRepository.GetAllImagesWithPrices();

            if (imagesWithPrices == null || !imagesWithPrices.Any())
            {
                return NotFound("No images with prices found");
            }

            return Ok(imagesWithPrices);
        }

        //[HttpGet("Search")]
        //public IActionResult GetVendorEvents(string city, string eventName, decimal price)
        //{
        //    // Parse eventName to EventName enum
        //    //if (!Enum.TryParse(eventName, out EventName selectedEvent))
        //    //{
        //    //    return BadRequest("Invalid event name.");
        //    //}

        //    //try
        //    //{
        //    //    var vendorEvents = _vendorEventRepository.GetVendorEvents(city, selectedEvent.ToString(), price);
        //    //    return Ok(vendorEvents.ToList());
        //    //}
        //    //catch (ArgumentException ex)
        //    //{
        //    //    // Handle invalid event name
        //    //    return BadRequest(ex.Message);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    // Log the exception
        //    //    return StatusCode(500, "An error occurred while processing your request.");
        //    //}
        //}


        //[HttpPost("AddingDetails")]
        //public async Task<IActionResult> AddEventvendorDetails(VendorEventModel model, ImageModel imageModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var addedModel = await _vendorEventRepository.AddEventvendorDetails(model, imageModel);
        //            return Ok(addedModel); // You can return the added model if needed
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log the exception
        //            return StatusCode(500, "Internal server error: " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}

        //[HttpPost("AddDetailstest")]
        //public async Task<ActionResult<VendorEventModel>> AddDetails(VendorEventModel vendorEventModel)
        //{
        //    try
        //    {
        //        var res = await _vendorEventRepository.AddEventvendorDetails();
        //        if (res != null)
        //        {
        //            return Ok(res);
        //        }
        //        else
        //        {
        //            return BadRequest(new { Success = false, Message = "fail to add value"});
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Success = false, message = ex.Message });
        //    }
        //}

        //public class VendorDetailRequest
        //{
        //    public VendorEventModel VendorEventModel { get; set; }
        //    //public ImageModel ImageModelity { get; set; }

        //}

        [HttpPost("AddVendorEvent.")]
        public async Task<ActionResult> AddVendorEvent([FromForm] VendorEventModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest("vendor event is null");
            }
            //if ( == null || Imageurls.Count == 0)
            //{
            //    return BadRequest("Image url is required");
            //}
            try
            {
                bool res = await _vendorEventRepository.AddEventvendorDetails(viewModel, _cloudinary);
                if(res)
                {
                    return Ok("Decoration Added Successfullly........!");
                }
                else
                {
                    return BadRequest("fail to add vendor event");
                }
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = "server error....1" });
            }
        }

        [HttpGet("GetAllVendorEvents")]
        public async Task<IActionResult> GetAllVendors()
        {
            try
            {

                var get = await _vendorEventRepository.GetAllEventvendorDetails();
                if (get == null)
                {
                    return NotFound("not found......");

                }
                return Ok(get);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            try
            {
            var list=_vendorEventRepository.GetAllVendorEventImages();
            return Ok(list);

            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteVendorEvent(Guid Id)
        {
            try
            {
                var IsDeleted=await _vendorEventRepository.DeleteVendorById(Id);
                if (IsDeleted)
                {
                    return Ok("VendorEvent Deleted Successfully....!");
                }
                else
                {
                    return NotFound("VendorEvent Not Found");
                }
            }catch(Exception ex)
            {
                Console.WriteLine("ERROR DURING DELETION",ex.Message);
                return StatusCode(500, "An error occurred while deleting VendorEvent");
            }
        }
    }
}