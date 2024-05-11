using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Practice_01.Data;
using Practice_01.Models;
using Practice_01.ViewModel;
using System.Linq;

namespace Practice_01.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddBookingAsync(BookingModel bookingModel)
        {
            try
            {
                var booking = new Booking
                {
                    Id=bookingModel.BookingId,
                    UserId = bookingModel.UserId,
                    EventId = bookingModel.EventId,
                    Payment = (decimal)bookingModel.Payment,
                    EventLocation = bookingModel.EventLocation,
                    EventDate = bookingModel.EventDate,
                    PaymentStatus = bookingModel.PaymentStatus,
                    IsBooked = true,
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBookingAsync(Guid bookingId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                    return false;

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<BookingModel>> GetAllBookingsAsync()
        {
            var bookings = await _context.Bookings.ToListAsync();
            var bookingModels = bookings.Select(booking => new BookingModel
            {
                UserId = booking.UserId,
                EventId = booking.EventId,
                Payment = booking.Payment,
                EventLocation = booking.EventLocation,
                EventDate = booking.EventDate,
                IsBooked = booking.IsBooked
            }).ToList();

            return bookingModels;
        }

        public async Task<BookingModel> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
                return null;

            var bookingModel = new BookingModel
            {
                UserId = booking.UserId,
                EventId = booking.EventId,
                Payment = booking.Payment,
                EventLocation = booking.EventLocation,
                EventDate = booking.EventDate,
                IsBooked = booking.IsBooked
            };

            return bookingModel;
        }

        //get value base on userId
        public async Task<List<VendorEventModel>> GetVendorEventsUserID(Guid userId)
        {
            try
            {
                var data =_context.VendorEvents.
                        Include(x => x.Vendor)
                        .Include(x => x.Events)
                        .ThenInclude(x => x.Bookings)
                        .Select(y => new VendorEventModel
                        {
                            
                            EventName = y.Events.EventName,
                            Price = y.Price,
                            DishName = y.DishName,
                            BookingUserId = y.Events != null && y.Events.Bookings.Any() ? y.Events.Bookings.FirstOrDefault().UserId : null,
                            BookingEventDate = y.Events != null && y.Events.Bookings.Any() ? y.Events.Bookings.FirstOrDefault().EventDate : null,
                            FinalPayment = y.Events != null && y.Events.Bookings.Any() ? y.Events.Bookings.FirstOrDefault().Payment : null
                        }).ToList();

                return data;
            }catch(Exception ex) {
                throw new Exception("An error occurred while retrieving data: " + ex.Message);
            }
        }

        public async Task<bool> UpdateBookingAsync(Guid bookingId, BookingModel bookingModel)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                    return false;

                booking.UserId = bookingModel.UserId;
                booking.EventId = bookingModel.EventId;
                booking.Payment = (decimal)bookingModel.Payment;
                booking.EventLocation = bookingModel.EventLocation;
                booking.EventDate = bookingModel.EventDate;
                booking.IsBooked = bookingModel.IsBooked;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
