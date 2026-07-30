using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Interfaces;
using SmartInventory.Application.Services;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    private readonly IPdfService _pdfService;

    public InvoiceController(IInvoiceService invoiceService, IPdfService pdfService)
    {
        _invoiceService = invoiceService;
        _pdfService = pdfService;
    }

    [HttpGet("{saleId}")]
    public async Task<IActionResult> GetInvoice(int saleId)
    {
        var result = await _invoiceService.GetInvoiceAsync(saleId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> GeneratePdf(int id)
    {

        var invoice = await _invoiceService.GetInvoiceAsync(id);

        if (invoice == null)
            return NotFound();


        var pdf = _pdfService.GenerateInvoicePdf(invoice);


        return File(
            pdf,
            "application/pdf",
            $"Invoice-{invoice.InvoiceNumber}.pdf");

    }
}