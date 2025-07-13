using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class Report
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;

        public string Content { get; set; } = string.Empty;

        // Foreign Key
        public int UserId { get; set; } // Many-to-One with User

        // Navigation Property
        public User User { get; set; } = null!;
    }
}

