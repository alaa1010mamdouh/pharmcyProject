using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class Medicine
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string Category { get; set; } = "General"; // Antibiotics, Painkillers, etc.

        public string? Manufacturer { get; set; }

        public string? Barcode { get; set; }

        public bool IsPrescriptionRequired { get; set; } = false;

        
        public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>(); // One-to-Many
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>(); // One-to-Many

    }
}
