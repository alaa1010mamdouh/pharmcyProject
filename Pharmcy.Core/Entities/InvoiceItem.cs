using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // Foreign Keys
        public int InvoiceId { get; set; } // Many-to-One with Invoice
        public int MedicineId { get; set; } // Many-to-One with Medicine

        // Navigation Properties
        public Invoice Invoice { get; set; } = null!;
        public Medicine Medicine { get; set; } = null!;
    }
}
