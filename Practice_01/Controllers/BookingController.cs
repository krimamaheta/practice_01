using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Practice_01.Repository;
using Practice_01.ViewModel;

namespace Practice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _Bookingrepository;
        public BookingController(IBookingRepository Bookingrepository)
        {
            _Bookingrepository = Bookingrepository; 
        }
        [HttpPost("AddBook")]
        public async Task<IActionResult>AddBook(BookingModel bookingModel)
        {
            var book=await _Bookingrepository.AddBookingAsync(bookingModel);
            if (book)
            {
                return Ok("Booking added successfully");
            }
            else
            {
                return BadRequest("Faile to add booking");
            }
        }


        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult>getById(Guid Id)
        {
            var book=await _Bookingrepository.GetBookingByIdAsync(Id);
            if (book == null)
            {
                return BadRequest("booking not found");
            }
            else
            {
                return Ok(book);

            }
        }

        [HttpGet("AllBooking")]
        public async Task<IActionResult>GetAllBooking()
        {
            var book=await _Bookingrepository.GetAllBookingsAsync();
            if(book == null)
            {
                return BadRequest("No booking found");
            }
            else
            {
                return Ok(book);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult>DeleteBooking(Guid Id)
        {
            var book=await _Bookingrepository.DeleteBookingAsync(Id);
            if (book)
            {
                return Ok("Delete sucessfully");
            }
            else
            {
                return BadRequest("Booking not found");
            }
        }

        [HttpPut("update/{Id}")]
        public async Task<IActionResult>UpdateBooking(Guid Id,BookingModel bookingmodel)
        {
            var book=await _Bookingrepository.UpdateBookingAsync(Id,bookingmodel);
            if (book)
            {
                return Ok("Update successfully");
            }
            else
            {
                return NotFound("Booking not Found");
            }
        }

        [HttpGet("AllUserId")]
        public async Task<IActionResult>GetbyUserId(Guid userId)
        {
            try
            {
               var res=await _Bookingrepository.GetVendorEventsUserID(userId);
                if (res != null)
                {
                    return Ok(res);
                }
                else {
                    return NotFound();
                        
                        
                        }
                
            }
            catch(Exception ex)
            {
                return BadRequest("An error occurred while retrieving data: " + ex.Message); // Returning 400 Bad Request if an exception occurs
            }
        }
    }
}
