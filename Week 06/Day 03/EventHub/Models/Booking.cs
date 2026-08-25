namespace EventHub.Models;

public class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime BookingDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public User User { get; set; } = null!;

    public ICollection<BookingItem> BookingItems { get; set; }
        = new List<BookingItem>();
}