using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>(); 
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
