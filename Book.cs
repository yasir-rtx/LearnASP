using LearnASP.Models;

namespace Obscura.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string? Description { get; set; }
        public string Publisher { get; set; } = String.Empty;
        public decimal Price { get; set; } = 0.0M;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsAvailable { get; set; } = true;
        public bool IsForSale { get; set; } = true;

        // Foreign key
        public int AuthorId { get; set; }

        // Navigation property
        public Author Author { get; set; } = null!;

    }
}
