using Practice_01.ViewModel;

namespace Practice_01.Repository
{
    public interface IEventRepository
    {
        Task<IEnumerable<EventModel>> GetAll();
        Task<EventModel> GetById(Guid EventId);
        Task<EventModel>AddEvent(EventModel model);

        Task<EventModel> UpdateEvent(Guid EventId,EventModel model);
        Task<bool> DeleteEvent(Guid eventId);
    }
}
