using System.Formats.Asn1;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Models;

namespace ProductManager.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController (AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Products.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post (Product product)
        {

           await  _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id , Product product)
        {
            var prod = _context.Products.FirstOrDefault(x => x.Id == id);

            if (prod == null) return NotFound();

            prod.Name = product.Name;
            prod.Price = product.Price;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var prod = await _context.Products.FindAsync(id);
            if (prod == null) return NotFound();
            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        //private static List<Product> _products = new List<Product>();
        //[HttpGet]
        //public IActionResult Get() => Ok(_products);

        //[HttpGet("{id}")]

        //public IActionResult Get (int id)
        //{
        //    var product = _products.FirstOrDefault(x => x.Id == id);
        //    return product == null ? NotFound() : Ok(product);
        //}

        //[HttpPost]
        //public IActionResult Post (Product product)
        //{
        //    product.Id = _products.Count + 1;
        //    _products.Add(product);
        //    return CreatedAtAction(nameof(Get), new { id = product.Id }, product);

        //}

        //[HttpPut("{id}")]
        //public IActionResult Put (int id , Product product)
        //{
        //    var prod = _products.FirstOrDefault(x => x.Id == id);
        //    if (prod == null) return NotFound();
        //    prod.Name = product.Name;
        //    prod.Price = product.Price;
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult Delete (int id)
        //{
        //    var product = _products.FirstOrDefault(x => x.Id == id);
        //    if (product == null) return NotFound();
        //    _products.Remove(product);
        //    return NoContent();
        //}

    }
}
