using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Interfaces;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
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
}