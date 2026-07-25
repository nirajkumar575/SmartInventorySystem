using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs.Customer;
using SmartInventory.Application.Interfaces;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _customerService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        var result = await _customerService.CreateAsync(dto);

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCustomerDto dto)
    {
        await _customerService.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _customerService.DeleteAsync(id);

        return NoContent();
    }
}