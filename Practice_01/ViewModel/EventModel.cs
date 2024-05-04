using Practice_01.Models;

namespace Practice_01.ViewModel
{
    public class EventModel
    {
        public Guid? EventId { get; set; }
        public string EventName { get; set; }
        public string? Description { get; set; }
    }
}
