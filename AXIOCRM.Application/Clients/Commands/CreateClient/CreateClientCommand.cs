using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Clients.Commands.CreateClient
{
    public record CreateClientCommand(
     [Required, StringLength(50)] string Name,
     [Required, StringLength(50)] string LastName,
     DateTime? DateOfBirth,
     string Designation,
     string Status,
     string ProfilePic,
     [EmailAddress] string Email,
     [Required, StringLength(15)] string Phone,
     [StringLength(250)] string Address,
     string City,
     string Province,
     string PostalCode,
     string Country,
     string Department,
     string JobTitle,
     DateTime? HireDate,
     decimal? Salary,
     string WorkReferenceNumber,
     string OccupationGroup,
     string Manager,
     string EmploymentType,
     string Notes,
     string Division
 );
}
