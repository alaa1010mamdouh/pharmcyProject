using Pharmcy.Core.Entities;

namespace pharmcy_Project.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

      
}
}
