using System.ComponentModel.DataAnnotations;

namespace pharmcy_Project.DTO
{
    public class CreatePrescriptionItemDto
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Dosage { get; set; }
    }
}
