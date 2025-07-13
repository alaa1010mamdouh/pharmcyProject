namespace pharmcy_Project.DTO
{
    public class CreatePrescriptionDto
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public string Diagnosis { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int UserId { get; set; }
        public string DoctorName { get; set; }
        public ICollection<PrescriptionItemDto> Items { get; set; }
    }
}

