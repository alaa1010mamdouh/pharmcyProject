using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class Prescription
    {
        public int Id { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        public string Diagnosis { get; set; } = string.Empty;

        // Foreign Keys
        public int CustomerId { get; set; } // Many-to-One with Customer
        public int UserId { get; set; } // Many-to-One with User

        // Navigation Properties
        public Customer Customer { get; set; } = null!;
        public User User { get; set; } = null!;

        // One-to-Many with PrescriptionItems
        public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
    }
}
