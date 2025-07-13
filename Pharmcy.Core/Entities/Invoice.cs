using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        // Foreign Key
        public int CustomerId { get; set; } // Many-to-One with Customer

        // Navigation Properties
        public Customer Customer { get; set; } = null!;

        // One-to-Many with InvoiceItems
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}
