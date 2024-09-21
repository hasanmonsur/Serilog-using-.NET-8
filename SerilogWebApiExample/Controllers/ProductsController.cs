using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerilogWebApiExample.Contacts;
using SerilogWebApiExample.Models;

namespace SerilogWebApiExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation("GET request received at {Time}", DateTime.UtcNow);
            var products = await _productRepository.GetAllProducts();
            if (products.Count()<=0)
            {
                _logger.LogInformation("GET Response : No Data Found ");
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation("GET request received at {Time}", DateTime.UtcNow);
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                _logger.LogInformation("GET Response : No Data Found ");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var result = await _productRepository.AddProduct(product);
            if (result > 0) return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            product.Id = id;
            var result = await _productRepository.UpdateProduct(product);
            if (result > 0) return NoContent();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productRepository.DeleteProduct(id);
            if (result > 0) return NoContent();
            return BadRequest();
        }
    }

}
