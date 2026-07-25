using Microsoft.AspNetCore.Mvc;
using ProductInventory.API.Context;
using ProductInventory.API.DTOs;
using ProductInventory.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ProductInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products
                .Select(p => MapToDto(p))
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound($"Product with id {id} not found.");

            return Ok(MapToDto(product));
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                QuantityStock = dto.QuantityStock,
                Category = dto.Category
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, MapToDto(product));
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound($"Product with id {id} not found.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.QuantityStock = dto.QuantityStock;
            product.Category = dto.Category;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound($"Product with id {id} not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static ProductDto MapToDto(Product p)
        {
            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                QuantityStock = p.QuantityStock,
                Category = p.Category
            };
        }
    }
}
