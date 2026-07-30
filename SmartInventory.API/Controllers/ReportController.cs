using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Interfaces;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Manager")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IExcelExportService _excelService;

    public ReportController(IReportService reportService, IExcelExportService excelService)
    {
        _reportService = reportService;
        _excelService = excelService;
    }

    [HttpGet("sales")]
    public async Task<IActionResult> Sales([FromQuery] ReportQueryParameters request)
    {
        return Ok(await _reportService.GetSalesReportAsync(request));
    }

    [HttpGet("purchase")]
    public async Task<IActionResult> Purchase([FromQuery] ReportQueryParameters request)
    {
        return Ok(await _reportService.GetPurchaseReportAsync(request));
    }

    [HttpGet("stock")]
    public async Task<IActionResult> Stock()
    {
        return Ok(await _reportService.GetStockReportAsync());
    }

    [HttpGet("profit")]
    public async Task<IActionResult> Profit([FromQuery] ReportQueryParameters request)
    {
        return Ok(await _reportService.GetProfitReportAsync(request));
    }

    [HttpGet("sales/excel")]
    public async Task<IActionResult> SalesExcel([FromQuery] ReportQueryParameters request)
    {
        var data = await _reportService.GetSalesReportAsync(request);

        var file = _excelService
            .GenerateSalesReportExcel(data);


        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "SalesReport.xlsx");
    }
}