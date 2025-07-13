using Pharmcy.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace pharmcy_Project.DTO
{
    public class MedicineDto
    {
      
        public int Id { get; set; }


        public string Name { get; set; } = string.Empty;

    
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string Category { get; set; } = "General"; // Antibiotics, Painkillers, etc.

        public string? Manufacturer { get; set; }

        public string? Barcode { get; set; }

        public bool IsPrescriptionRequired { get; set; } = false;

       

    }
}
