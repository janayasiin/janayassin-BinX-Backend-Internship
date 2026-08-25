namespace EventHub.Models;

public class TicketType
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int QuantityAvailable { get; set; }

    public Event Event { get; set; } = null!;

    public ICollection<BookingItem> BookingItems { get; set; }
        = new List<BookingItem>();
}