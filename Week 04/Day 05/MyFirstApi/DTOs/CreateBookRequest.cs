namespace MyFirstApi.DTOs;

public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }
}