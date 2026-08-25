using EventHub.DTOs.Responses;
using EventHub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<EventResponse>>> GetAll(
        int page = 1,
        int pageSize = 10,
        int? categoryId = null,
        string? location = null,
        string? sort = null)
    {
        var result = await _eventService.GetEventsAsync(
            page,
            pageSize,
            categoryId,
            location,
            sort);

        return Ok(result);
    }
}