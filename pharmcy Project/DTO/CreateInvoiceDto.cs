using System.ComponentModel.DataAnnotations;

namespace pharmcy_Project.DTO
{
    public class CreateInvoiceDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public ICollection<InvoiceItemDto> Items { get; set; }

    }
}
