using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs.Purchase;
using SmartInventory.Application.Interfaces;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
    }
}
