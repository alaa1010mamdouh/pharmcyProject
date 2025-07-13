namespace pharmcy_Project.DTO
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public ICollection<InvoiceItemDto> InvoiceItems { get; set; }
    }
}
