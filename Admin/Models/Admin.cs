using System.ComponentModel.DataAnnotations;

namespace studySpaceWebApp.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;
    }
}
