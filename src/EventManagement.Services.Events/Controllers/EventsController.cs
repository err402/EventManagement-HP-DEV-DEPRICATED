using EventManagement.Core.Models;
using EventManagement.Core.DTOs;
using EventManagement.Services.Events.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Services.Events.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EventsController> _logger;

    public EventsController(ApplicationDbContext context, ILogger<EventsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent(CreateEventDto dto)
    {
        try
        {
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                IsDraft = true
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new event with ID: {EventId}", newEvent.Id);
            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, newEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return StatusCode(500, "An error occurred while creating the event");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        try
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving events");
            return StatusCode(500, "An error occurred while retrieving events");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(Guid id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
        {
            _logger.LogWarning("Event not found with ID: {EventId}", id);
            return NotFound();
        }
        return eventItem;
    }
}