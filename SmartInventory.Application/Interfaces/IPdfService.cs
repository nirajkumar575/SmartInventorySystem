using SmartInventory.Application.DTOs.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Application.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateInvoicePdf(InvoiceDto invoice);
    }
}
