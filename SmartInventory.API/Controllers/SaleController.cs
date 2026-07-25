using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs.Sale;
using SmartInventory.Application.Interfaces;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SaleController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SaleQueryParameters request)
    {
        return Ok(await _saleService.GetAllAsync(request));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _saleService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSaleDto dto)
    {
        var result = await _saleService.CreateAsync(dto);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _saleService.DeleteAsync(id);

        return NoContent();
    }
}