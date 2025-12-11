using Obscura.Models;

namespace LearnASP.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation property
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
