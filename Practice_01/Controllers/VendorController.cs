using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Practice_01.Models;
using Practice_01.Repository;
using Practice_01.ViewModel;

namespace Practice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorRepository  _vendorRepository;
       
        public VendorController(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;   
         
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<VendorModel>>>GetAll()
        {
            var vendor=await _vendorRepository.GetAllVendor();
            if(vendor == null)
            {
                return NotFound("vendor not found");
            }
            return Ok(vendor);
        }

        [HttpGet("get/{Id}")]
        public async Task<ActionResult<VendorModel>> GetVendorModelById(Guid Id)
        {
            var vendorModel = await _vendorRepository.GetVendorById(Id);
            if (vendorModel == null)
            {
                return NotFound(); // VendorModel not found
            }
            return Ok(vendorModel);
        }

        [HttpGet("getByUserId/{userId}")]
        public async Task<ActionResult<VendorModel>> GetVendorModelByUserId(string userId)
        {
            Console.WriteLine("---------------"+userId);
            var vendorModel = await _vendorRepository.GetVendorByUserId(userId);
            if (vendorModel == null)
            {
                return NotFound(); // VendorModel not found
            }
            return Ok(vendorModel);
        }


        [HttpDelete("Delete/{Id}")]
        public async Task<ActionResult<bool>>Deletevendor(Guid Id)
        {
            var del=await _vendorRepository.DeleteVendorAsync(Id);
            if (!del)
            {
                return StatusCode(500, "Failed to delete the vendor.");

            }
            return Ok(new {message="Vendor Deleted Successfully"});
        }

        [HttpPost("AddVendor/{userId}")]
        public async Task<ActionResult<VendorModel>>AddVendor(VendorModel vendor,Guid userId)
        {
            var createvendor=await _vendorRepository.AddVendorAsync(vendor,userId);
            Console.WriteLine("----"+createvendor);
            return CreatedAtAction(null, new {createvendor.VendorId},createvendor);
           // return CreatedAtAction(null,createvendor);
        }

        [HttpPut("Update/{Id}")]
        public async Task<ActionResult<VendorModel>> UpdateVendorModel(Guid Id, VendorModel updatedVendorModel)
        {
            var result = await _vendorRepository.UpdateVendorAsync(Id,updatedVendorModel);
            if (result == null)
            {
                return StatusCode(500, "Fail to update Vendor.....!");// VendorModel not found
            }
            return Ok(new {message="Vendor Updated Successfully.......!"});
        }

        [HttpGet("alltype")]

        public async Task<IActionResult> GetAllvendorType()
        {
            try
            {
                var vendortype = await _vendorRepository.GetAlTypeOfvendor();
                return Ok(vendortype);  
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
           
        }





    }
}
