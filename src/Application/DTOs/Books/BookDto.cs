namespace LearnASP.Application.DTOs.Books
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsForSale { get; set; }

        // Author Information
        public int AuthorId { get; set; }
        public string? AuthorFullName { get; set; }
    }
}
