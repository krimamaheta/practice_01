using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Practice_01.Data;
using Practice_01.Models;
using Practice_01.ViewModel;

namespace Practice_01.Repository
{
    public class EventRepository:IEventRepository
    {
        private readonly ApplicationDbContext _context;
        public EventRepository(ApplicationDbContext context)
        {
            _context = context;   
        }

        public async Task<EventModel> AddEvent(EventModel model)
        {
            var newEvent = new Event
            {
               
                EventName = model.EventName,
                Description = model.Description,
            };
            _context.Add(newEvent);
            await _context.SaveChangesAsync();

            return model;

        }

        public async Task<bool> DeleteEvent(Guid eventId)
        {
            var exist=await _context.Events.FindAsync(eventId);
            if(exist == null) {
                return false;
            }
             _context.Events.Remove(exist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EventModel>> GetAll()
        {
            var events = await _context.Events.ToListAsync();
            return events.Select(e => new EventModel { EventId=e.Id, EventName = e.EventName, Description = e.Description, });
        }

        public async Task<EventModel> GetById(Guid EventId)
        {
            var existingEvent = await _context.Events.FindAsync(EventId);
            if (existingEvent == null)
            {
                return null; // Handle case where event is not found
            }

            var eventModel = new EventModel
            {
                EventId = existingEvent.Id,
                EventName = existingEvent.EventName,
                Description = existingEvent.Description
                // You may need to map other properties as well
            };

            return eventModel;
        }

        public async Task<EventModel> UpdateEvent(Guid EventId, EventModel model)
        {
           var exist=await _context.Events.FindAsync(EventId);
            if (exist == null) {
                return null;
            }

            exist.EventName = model.EventName;
            exist.Description = model.Description;
            await _context.SaveChangesAsync();
            return model;
        }
    }
}
