using AXIOCRM.Application.Interfaces;
using AXIOCRM.Domain.Entities.Invoice;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AXIOCRM.Application.Services
{
    public class DeliveryMethodService : IDeliveryMethod
    {

        private readonly AppDbContext _context;

        public DeliveryMethodService(AppDbContext context)
        {
            _context = context;
        }

     
        public  async Task<DeliveryMethod> CreateAsync(DeliveryMethod deliveryMethod)
        {
            _context.DeliveryMethods.Add(deliveryMethod);
            await _context.SaveChangesAsync();
            return deliveryMethod;
        }
        

        public async Task<bool> DeleteAsync(int id)
        {
            var deliveryMethod = await _context.DeliveryMethods.FindAsync(id);
            if (deliveryMethod == null) return false;

            _context.DeliveryMethods.Remove(deliveryMethod);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DeliveryMethod>> GetAllAsync()
         => await _context.DeliveryMethods.ToListAsync();

        public async Task<DeliveryMethod> GetByIdAsync(int id)
         => await _context.DeliveryMethods.FindAsync(id);

    
        public async Task<DeliveryMethod> UpdateAsync(int id, DeliveryMethod deliveryMethod)
        {
            var existing = await _context.DeliveryMethods.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (existing == null) return null;

            deliveryMethod.Id = id;
            _context.Entry(deliveryMethod).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return deliveryMethod;
        }
    }
}
