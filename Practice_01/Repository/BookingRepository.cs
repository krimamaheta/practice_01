using Microsoft.EntityFrameworkCore;
using Practice_01.Data;
using Practice_01.Models;
using Practice_01.ViewModel;

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
                    UserId = bookingModel.UserId,
                    EventId = bookingModel.EventId,
                    Payment = (decimal)bookingModel.Payment,
                    EventLocation = bookingModel.EventLocation,
                    EventDate = bookingModel.EventDate,
                    IsBooked = bookingModel.IsBooked
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
