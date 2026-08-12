namespace MyFirstApi.Models
{
    public class Member
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime JoinedDate { get; set; }


        // Navigation Property
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
