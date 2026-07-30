using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Domain.Entities
{
    public class StockAdjustment : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }

        // Increase / Decrease
        public string AdjustmentType { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime AdjustmentDate { get; set; } = DateTime.UtcNow;
    }
}
