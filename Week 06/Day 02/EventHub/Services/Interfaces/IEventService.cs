using EventHub.DTOs.Responses;

namespace EventHub.Services.Interfaces;

public interface IEventService
{
    Task<PagedResult<EventResponse>> GetEventsAsync(
     int page,
     int pageSize,
     int? categoryId,
     string? location,
     string? sort);
}