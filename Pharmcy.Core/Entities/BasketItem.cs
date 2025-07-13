using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }

        public string Category { get; set; } = "General";
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }

    }
}
