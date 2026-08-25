namespace EventHub.DTOs.Responses;

public class EventResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Capacity { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}