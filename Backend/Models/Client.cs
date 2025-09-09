using System.ComponentModel.DataAnnotations;
using ProductManager.API.Validations;

namespace ProductManager.API.Models
{
    public class Client : IValidatableObject
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{Name} {LastName}".Trim();

        [NotInFuture(ErrorMessage = "The date of birth cannot be in the future.")]

        public DateTime? DateOfBirth { get; set; }
        public string Designation { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ProfilePic{ get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }



        [StringLength(250)]
        public string Address { get; set; } = string.Empty;     
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;  
        public string JobTitle { get; set; } = string.Empty;   
        public DateTime? HireDate { get; set; }               
        public decimal? Salary { get; set; }
        public string WorkReferenceNumber { get; set; } = string.Empty;
        public string OccupationGroup { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;     
        public string EmploymentType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;


        public ICollection<Order> Orders { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           if(DateOfBirth.HasValue && DateOfBirth.Value > DateTime.Today)
            {
                yield return new ValidationResult("The date of birth cannot be in the future.", new[] { nameof(DateOfBirth) });
            }
            if (HireDate.HasValue && HireDate.Value > DateTime.Today)
            {
                yield return new ValidationResult("The hire date cannot be in the future.", new[] { nameof(HireDate) });
            }
        }



        //public ICollection<ClientProduct> ClientProducts { get; set; } = new List<ClientProduct>();

    }
}
