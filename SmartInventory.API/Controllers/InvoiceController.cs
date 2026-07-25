using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Interfaces;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet("{saleId}")]
    public async Task<IActionResult> GetInvoice(int saleId)
    {
        var result = await _invoiceService.GetInvoiceAsync(saleId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}