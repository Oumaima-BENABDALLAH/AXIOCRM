using AXIOCRM.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AXIOCRM.Application.Clients.Commands.DeleteClient
{
    public class DeleteClientCommandHandler
    {
        private readonly AppDbContext _context;
        public DeleteClientCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> HandleAsync(int id)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(10)
            };

            using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            transactionOptions,
            TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var client = await _context.Clients.FindAsync(id);
                if (client == null) return false;
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                scope.Complete();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
