using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Hubs;
using ProductManager.API.Models;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Services
{
    public class ProductService : IProductService
    {

        private readonly AppDbContext _context;


        public ProductService(AppDbContext context)
        {
            _context = context;
           
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products.Include(c => c.OrderProducts).ToListAsync();

        public async Task<Product> GetByIdAsync(int id)
            => await _context.Products.Include(c => c.OrderProducts).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(int id, Product updated)
        {
            var existing = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null) return null;

            updated.Id = id;

            _context.Entry(updated).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return updated;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

       
    }
}
