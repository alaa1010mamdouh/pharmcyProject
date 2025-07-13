using System.ComponentModel.DataAnnotations;

namespace pharmcy_Project.DTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
