using System.ComponentModel.DataAnnotations;

namespace LearnASP.Application.DTOs.Books
{
    public class UpdateBookRequest
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
        [Range(0.01, double.MaxValue, ErrorMessage = "Harga harus lebih besar dari nol.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "ID Penulis diperlukan.")]
        public int AuthorId { get; set; }

        public bool IsAvailable { get; set; }
        public bool IsForSale { get; set; }
    }
}
