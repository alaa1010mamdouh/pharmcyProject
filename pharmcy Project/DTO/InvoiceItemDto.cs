using System.ComponentModel.DataAnnotations;

namespace pharmcy_Project.DTO
{
    public class InvoiceItemDto
    {
        public int Id { get; set; }

        [Required]
        public int MedicineId { get; set; }

        public string MedicineName { get; set; }   

        [Range(1, int.MaxValue, ErrorMessage = "الكمية يجب أن تكون على الأقل 1")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "السعر يجب أن يكون أكبر من الصفر")]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;
    }
    }
