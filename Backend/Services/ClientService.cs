using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Models;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Services
{
    public class ClientService :IClientService
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
             => await _context.Clients.ToListAsync();

        public async Task<Client> GetByIdAsync(int id)
            => await _context.Clients.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Client> GetClientWithOrdersAsync(int id)
          => await _context.Clients.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id);
        public async Task<Client> CreateAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }
        public async Task<Client> UpdateAsync(int id, Client updated)
        {
            var existing = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (existing == null) return null;

            updated.Id = id;
            _context.Entry(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return updated;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
