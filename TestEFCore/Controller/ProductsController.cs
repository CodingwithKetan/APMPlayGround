using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestEFCore.Data;
using TestEFCore.Models;

namespace TestEFCore.Controller;

     [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        
        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _dbContext.Products.Any(e => e.Id == id);
        }
    }