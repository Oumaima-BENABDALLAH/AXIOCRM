using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Application.Orders.Commands.CreateOrder;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AXIOCRM.Application.Clients.Commands.CreateClient
{
    public class CreateClientCommandHandler

    {

        private readonly AppDbContext _context;

        public CreateClientCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ClientDto> HandleAsync(CreateClientCommand command)
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

                var client = new Client
                {
                    Name = command.Name,
                    LastName = command.LastName,
                    DateOfBirth = command.DateOfBirth,
                    Designation = command.Designation,
                    Status = command.Status,
                    ProfilePic = command.ProfilePic,
                    Email = command.Email,
                    Phone = command.Phone,
                    Address = command.Address,
                    City = command.City,
                    Province = command.Province,
                    PostalCode = command.PostalCode,
                    Country = command.Country,
                    Department = command.Department,
                    JobTitle = command.JobTitle,
                    HireDate = command.HireDate,
                    Salary = command.Salary,
                    WorkReferenceNumber = command.WorkReferenceNumber,
                    OccupationGroup = command.OccupationGroup,
                    Manager = command.Manager,
                    EmploymentType = command.EmploymentType,
                    Notes = command.Notes,
                    Division = command.Division

                };

                _context.Clients.Add(client);


                await _context.SaveChangesAsync();



                // Mise à jour du montant total de la commande
                
                await _context.SaveChangesAsync();
                scope.Complete();

                return client.ToDto();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la création de client : {ex.Message}");
            }
        }
    }
}
