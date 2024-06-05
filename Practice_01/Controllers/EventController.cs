using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practice_01.Repository;
using Practice_01.ViewModel;

namespace Practice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        [HttpGet("AllEvent")]   
        public async Task<ActionResult<IEnumerable<EventModel>>> GetAll(int page = 1, int pageSize = 5)
        {
                var e= await _eventRepository.GetAll(page,pageSize);
                 return Ok(e);
        }


        [HttpGet("AllEvents")]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetAllEvents()
        {
            var e = await _eventRepository.GetAllEvents();
            return Ok(e);
        }




        [HttpGet("get/{Id}")]
     
        public async Task<ActionResult<EventModel>> GetEventById(Guid Id)
        {            
            var @event = await _eventRepository.GetById(Id);
            if (@event == null)
            {
                return NotFound(); // Event not found
            }
            return Ok(@event);
        }

        [HttpPut("update/{Id}")]
        public async Task<IActionResult>UpdateEvent(Guid Id,EventModel eventModel)
        {
            var updateEvent=await _eventRepository.UpdateEvent(Id, eventModel);
            if(updateEvent == null)
            {
                return NotFound();
            }    
            return Ok(new { message = "Event Update Successfully.....!" });
        }

        [HttpPost("AddEvent")]
        public async Task<ActionResult<EventModel>>AddEvent(EventModel eventModel)
        {
            var create=await _eventRepository.AddEvent(eventModel);
            return CreatedAtAction(null,new {Id=create.EventId},create);
        }
        [HttpDelete("DeleteEvent/{Id}")]
        public async Task<IActionResult> DeleteEvent(Guid Id)
        {
            Console.WriteLine("hell------1212 " + Id);
            var result = await _eventRepository.DeleteEvent(Id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { message = "Event Deleted Successfully...!" });
        }


    }
}
