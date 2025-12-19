using System.ComponentModel.DataAnnotations;

namespace LearnASP.Application.DTOs.Authors
{
    public class UpdateAuthorRequest
    {
        [Required(ErrorMessage = "Nama lengkap penulis diperlukan.")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Biography { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(100)]
        public string? Nationality { get; set; }
    }
}
