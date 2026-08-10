using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.DTOs
{
    public class CreateBookRequest
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]

        public int AuthorId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]


        public int CategoryId { get; set; }
    }
}
