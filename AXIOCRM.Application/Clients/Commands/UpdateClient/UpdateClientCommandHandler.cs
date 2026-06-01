using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Application.Orders.Commands.UpdateOrder;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AXIOCRM.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandHandler
    {

        private readonly AppDbContext _context;

        public UpdateClientCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ClientDto?> HandleAsync(UpdateClientCommand command)
        {

            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(15)
            };

            using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            transactionOptions,
            TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var existing = await _context.Clients.FirstOrDefaultAsync(c => c.Id == command.Id);

                if (existing == null) return null;

                existing.Name = command.Name;
                existing.LastName = command.LastName;
                existing.DateOfBirth = command.DateOfBirth;
                existing.Designation = command.Designation;
                existing.Status = command.Status;
                existing.ProfilePic = command.ProfilePic;
                existing.Email = command.Email;
                existing.Phone = command.Phone;
                existing.Address = command.Address;
                existing.City = command.City;
                existing.Province = command.Province;
                existing.PostalCode = command.PostalCode;
                existing.Country = command.Country;
                existing.Department = command.Department;
                existing.JobTitle = command.JobTitle;
                existing.Manager = command.Manager;
                existing.EmploymentType = command.EmploymentType;
                existing.Notes = command.Notes;
                existing.Division = command.Division;
                existing.Status = command.Status;

               
                await _context.SaveChangesAsync();
                scope.Complete();
                return existing.ToDto();

            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw; // Rethrow to be handled by upper layers
            }
        }
    }
}
