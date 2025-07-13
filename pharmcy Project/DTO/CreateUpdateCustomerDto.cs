using System.ComponentModel.DataAnnotations;

namespace pharmcy_Project.DTO
{
    public class CreateUpdateCustomerDto
    {
        [Required]
        public string Name { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

    }
}
