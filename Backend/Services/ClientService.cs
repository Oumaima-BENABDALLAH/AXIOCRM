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

        /*   public async Task<Client> UpdateAsync(int id, Client updated)
           {
               var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);//FindAsync(id);
               if (client == null) return null;
               updated.Id = id;
               client.Name = updated.Name;
               client.LastName = updated.LastName;
               client.Email = updated.Email;
               client.Phone = updated.Phone;
               client.Designation = updated.Designation;
               client.Status = updated.Status;
               client.ProfilePic = updated.ProfilePic;
               client.Address = updated.Address;
               client.City = updated.City;
               client.Country = updated.Country;
               client.Department = updated.Department;
               client.JobTitle = updated.JobTitle;
               client.HireDate = updated.HireDate;
               client.Salary = updated.Salary;
               client.Manager = updated.Manager;
               client.EmploymentType = updated.EmploymentType;
               client.Notes = updated.Notes;
               client.Division = updated.Division;
               client.PostalCode = updated.PostalCode;
               client.Province = updated.Province;
               client.DateOfBirth = updated.DateOfBirth;
               client.OccupationGroup = updated.OccupationGroup;
               client.WorkReferenceNumber = updated.WorkReferenceNumber;
               _context.Entry(updated).State = EntityState.Modified;
               await _context.SaveChangesAsync();
               return client;
           }*/

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
