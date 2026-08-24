namespace EventHub.Models;

public class BookingItem
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int TicketTypeId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public Booking Booking { get; set; } = null!;

    public TicketType TicketType { get; set; } = null!;
}