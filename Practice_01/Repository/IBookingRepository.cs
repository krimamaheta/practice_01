using Practice_01.ViewModel;

namespace Practice_01.Repository
{
    public interface IBookingRepository
    {
        Task<bool> AddBookingAsync(BookingModel bookingModel);
        Task<bool> UpdateBookingAsync(Guid bookingId, BookingModel bookingModel);
        Task<bool> DeleteBookingAsync(Guid bookingId);
        Task<BookingModel> GetBookingByIdAsync(Guid bookingId);
        Task<IEnumerable<BookingModel>> GetAllBookingsAsync();

        //booking with payment
        //Task<BookingModel> GetAllPaymentWithBookingAsync();

        //find by userid
        Task<List<VendorEventModel>> GetVendorEventsUserID(Guid userId);
    }
}
