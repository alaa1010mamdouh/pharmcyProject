using System.ComponentModel.DataAnnotations;

namespace pharmcy_Project.DTO
{
    public class PrescriptionItemDto
    {
        public int Id { get; set; }

        [Required]
        public string Dosage { get; set; }

        [Required]
        public int MedicineId { get; set; }

        public string MedicineName { get; set; }
        public string MedicineDescription { get; set; }
        public decimal MedicinePrice { get; set; }
    }
}
