using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs.Purchase;
using SmartInventory.Application.Interfaces;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PurchaseQueryParameters request)
        {
            var result = await _purchaseService.GetAllAsync(request);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePurchaseDto dto)
        {
            var result = await _purchaseService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _purchaseService.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePurchaseDto dto)
        {
            var updated = await _purchaseService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _purchaseService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
