using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Extensions
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CostValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) 
            { 
                int cost = (int)value;
                if(cost %5 != 0)
                {
                    return new ValidationResult("Cost must be in multiples of 5. ");
                }
            }

            return ValidationResult.Success;


        }
    }
}
