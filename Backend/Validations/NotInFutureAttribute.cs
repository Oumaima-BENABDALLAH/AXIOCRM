using System.ComponentModel.DataAnnotations;

namespace ProductManager.API.Validations
{
    public class NotInFutureAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date <= DateTime.Today; // interdit les dates > aujourd’hui
            }
            return true; // null est accepté (si optionnel)
        }
    }
}
