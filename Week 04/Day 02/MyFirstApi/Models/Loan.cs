namespace MyFirstApi.Models
{
    public class Loan
    {
        public int Id { get; set; }


        // Foreign Keys
        public int MemberId { get; set; }

        public int BookId { get; set; }


        public DateTime LoanDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }


        // Navigation Properties
        public Member Member { get; set; } = null!;

        public Book Book { get; set; } = null!;
    }
}
