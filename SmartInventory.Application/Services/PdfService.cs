using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SmartInventory.Application.DTOs.Invoice;
using SmartInventory.Application.Interfaces;

namespace SmartInventory.Application.Services
{
    public class PdfService : IPdfService
    {

        public byte[] GenerateInvoicePdf(InvoiceDto invoice)
        {

            var document = Document.Create(container =>
            {

                container.Page(page =>
                {

                    page.Size(PageSizes.A4);


                    page.Content()
                    .Column(column =>
                    {

                        column.Item()
                        .Text("Smart Inventory Invoice")
                        .FontSize(20);


                        column.Item()
                        .Text($"Invoice No : {invoice.InvoiceNumber}");


                        column.Item()
                        .Text($"Customer : {invoice.CustomerName}");


                        foreach (var item in invoice.Items)
                        {
                            column.Item()
                            .Text(
                            $"{item.ProductName}  {item.Quantity}  {item.TotalPrice}");
                        }


                        column.Item()
                        .Text($"Total : {invoice.TotalAmount}");

                    });


                });

            });


            return document.GeneratePdf();

        }

    }
}
