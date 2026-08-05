namespace MyFirstApi.Models
{
    public class Book
    {

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public decimal Price { get; set; }


        // Foreign Keys
        public int AuthorId { get; set; }

        public int CategoryId { get; set; }


        // Navigation Properties
        public Author Author { get; set; } = null!;

        public Category Category { get; set; } = null!;


        // Navigation Property
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}

