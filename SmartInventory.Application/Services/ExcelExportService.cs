using ClosedXML.Excel;
using SmartInventory.Application.DTOs.Reports;

public class ExcelExportService : IExcelExportService
{
    public byte[] GenerateSalesReportExcel(
        IEnumerable<SalesReportDto> data)
    {
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Sales Report");


        worksheet.Cell(1, 1).Value = "Invoice";
        worksheet.Cell(1, 2).Value = "Customer";
        worksheet.Cell(1, 3).Value = "Date";
        worksheet.Cell(1, 4).Value = "Amount";


        int row = 2;

        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.InvoiceNumber;
            worksheet.Cell(row, 2).Value = item.CustomerName;
            worksheet.Cell(row, 3).Value = item.SaleDate;
            worksheet.Cell(row, 4).Value = item.TotalAmount;

            row++;
        }


        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    public byte[] GeneratePurchaseReportExcel(
        IEnumerable<PurchaseReportDto> data)
    {
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Purchase Report");


        // Header
        worksheet.Cell(1, 1).Value = "Invoice Number";
        worksheet.Cell(1, 2).Value = "Supplier Name";
        worksheet.Cell(1, 3).Value = "Purchase Date";
        worksheet.Cell(1, 4).Value = "Total Amount";


        int row = 2;

        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.InvoiceNumber;
            worksheet.Cell(row, 2).Value = item.SupplierName;
            worksheet.Cell(row, 3).Value = item.PurchaseDate;
            worksheet.Cell(row, 4).Value = item.TotalAmount;

            row++;
        }


        worksheet.Columns().AdjustToContents();


        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }



    public byte[] GenerateStockReportExcel(
        IEnumerable<StockReportDto> data)
    {
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Stock Report");


        // Header
        worksheet.Cell(1, 1).Value = "Product Id";
        worksheet.Cell(1, 2).Value = "Product Name";
        worksheet.Cell(1, 3).Value = "Category";
        worksheet.Cell(1, 4).Value = "Quantity";
        worksheet.Cell(1, 5).Value = "Price";
        worksheet.Cell(1, 6).Value = "Low Stock";


        int row = 2;


        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.ProductId;
            worksheet.Cell(row, 2).Value = item.ProductName;
            worksheet.Cell(row, 3).Value = item.CategoryName;
            worksheet.Cell(row, 4).Value = item.Quantity;
            worksheet.Cell(row, 5).Value = item.Price;
            worksheet.Cell(row, 6).Value = item.IsLowStock ? "Yes" : "No";

            row++;
        }


        worksheet.Columns().AdjustToContents();


        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}