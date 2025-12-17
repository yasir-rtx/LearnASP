using System.ComponentModel.DataAnnotations;

namespace LearnASP.Application.DTOs.Authors
{
    public class CreateAuthorRequest
    {
        [Required(ErrorMessage = "Nama lengkap penulis diperlukan.")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Biography { get; set; }

        // Pengecekan tanggal lahir, bisa menggunakan validasi kustom jika perlu
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(100)]
        public string? Nationality { get; set; }
    }
}
