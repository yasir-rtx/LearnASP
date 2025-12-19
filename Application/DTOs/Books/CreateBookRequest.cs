using System.ComponentModel.DataAnnotations;

namespace LearnASP.Application.DTOs.Books
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "Judul buku diperlukan.")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Penerbit diperlukan.")]
        [MaxLength(100)]
        public string Publisher { get; set; } = string.Empty;

        [Required(ErrorMessage = "Harga diperlukan.")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Harga harus lebih besar dari nol.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "ID Penulis diperlukan.")]
        public int AuthorId { get; set; }

        // Default values bisa diatur di Service Layer atau di sini
        public bool IsAvailable { get; set; } = true;
        public bool IsForSale { get; set; } = true;
    }
}
