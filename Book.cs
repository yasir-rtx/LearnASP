using System.ComponentModel.DataAnnotations;

namespace Obscura.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }     
        
        [Required]    
        [MaxLength(200)]
        public string Title { get; set; } = String.Empty;

        [Required]
        [MaxLength(100)]
        public string Author { get; set; } = String.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Publisher { get; set; } = String.Empty;

        [Required]
        [Range(0.0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; } = 0.0M;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public int CreatedBy { get; set; }
        [MaxLength(100)]
        public int UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsAvailable { get; set; } = true;
        public bool IsForSale { get; set; } = true;

    }
}
