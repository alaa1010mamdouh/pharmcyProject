using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class Customer
    {
        public int Id { get; set; }
   
        public string Name { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>(); 
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>(); // One-to-Many
    }
}
