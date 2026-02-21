using System.ComponentModel.DataAnnotations;

namespace ProductManager.API.Validations
{
    public class NotInFutureAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date <= DateTime.Today; 
            }
            return true; 
        }
    }
}
