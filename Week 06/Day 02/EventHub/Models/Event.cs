using System.Net.Sockets;

namespace EventHub.Models;

public class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Location { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Capacity { get; set; }

    public int CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<TicketType> TicketTypes { get; set; }
        = new List<TicketType>();
}