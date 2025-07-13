using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class PrescriptionItem
    {
        public int Id { get; set; }

        public string Dosage { get; set; } = string.Empty;

        // Foreign Keys
        public int PrescriptionId { get; set; } // Many-to-One with Prescription
        public int MedicineId { get; set; } // Many-to-One with Medicine

        // Navigation Properties
        public Prescription Prescription { get; set; } = null!;
        public Medicine Medicine { get; set; } = null!;
    }
}
