using EventHub.Data;
using EventHub.DTOs.Responses;
using EventHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<EventResponse>> GetEventsAsync(
        int page,
        int pageSize,
        int? categoryId,
        string? location,
        string? sort)
    {
        var query = _context.Events
            .AsNoTracking()
            .AsQueryable();

        // Filter by category
        if (categoryId.HasValue)
        {
            query = query.Where(e => e.CategoryId == categoryId.Value);
        }

        // Filter by location
        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(e => e.Location.Contains(location));
        }

        // Sorting
        sort = sort?.ToLower();

        query = sort switch
        {
            "startdate_desc" => query.OrderByDescending(e => e.StartDate),
            _ => query.OrderBy(e => e.StartDate)
        };

        // Count after applying filters
        var totalCount = await query.CountAsync();

        // Project to DTO and apply pagination
        var items = await query
            .Select(e => new EventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Location = e.Location,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Capacity = e.Capacity,
                CategoryId = e.CategoryId,
                CategoryName = e.Category.Name
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize
        );

        return new PagedResult<EventResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }
}