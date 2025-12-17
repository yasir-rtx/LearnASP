namespace LearnASP.Application.DTOs.Authors
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
    }
}
