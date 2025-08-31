using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductsController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{code}")]
        public IActionResult Get(string code)
        {
            var product = _service.Get(code);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_service.Create(product)) return Conflict("Product code must be unique");
            return CreatedAtAction(nameof(Get), new { code = product.Code }, product);
        }

        [HttpPut("{code}")]
        public IActionResult Update(string code, Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_service.Update(code, product)) return NotFound();
            return NoContent();
        }

        [HttpDelete("{code}")]
        public IActionResult Delete(string code)
        {
            if (!_service.Delete(code)) return NotFound();
            return NoContent();
        }
    }
}
