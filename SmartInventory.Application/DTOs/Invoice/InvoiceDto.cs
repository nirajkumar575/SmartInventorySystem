using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Application.DTOs.Invoice
{
    public class InvoiceDto
    {
        public string InvoiceNumber { get; set; } = "";
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public string CustomerAddress { get; set; } = "";
        public decimal TotalAmount { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}
