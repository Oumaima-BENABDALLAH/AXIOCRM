using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Clients.Commands.UpdateClient
{
    public record   UpdateClientCommand
    {
        public int Id { get; init; }
        [Required, StringLength(50)] public string Name { get; init; } = default!;
        [Required, StringLength(50)] public string LastName { get; init; } = default!;
        public DateTime? DateOfBirth { get; init; }
        public string Designation { get; init; }
        public string Status { get; init; }
        public string ProfilePic { get; init; }
        [EmailAddress] public string Email { get; init; }
        [Required, StringLength(15)] public string Phone { get; init; }
        public string Address { get; init; }
        public string City { get; init; }
        public string Province { get; init; }
        public string PostalCode { get; init; }
        public string Country { get; init; }
        public string Department { get; init; }
        public string JobTitle { get; init; }
        public DateTime? HireDate { get; init; }
        public decimal? Salary { get; init; }
        public string WorkReferenceNumber { get; init; }
        public string OccupationGroup { get;  init; }
        public string Manager { get; init; }
        public string EmploymentType { get; init; }
        public string Notes { get; init; }
        public string Division { get; init; }
    }
}
