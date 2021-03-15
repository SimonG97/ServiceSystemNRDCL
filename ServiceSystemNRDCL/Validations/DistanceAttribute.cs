using ServiceSystemNRDCL.Models;
using System.ComponentModel.DataAnnotations;

namespace ServiceSystemNRDCL.Validations
{
    public class DistanceAttribute : ValidationAttribute
    {
        public double MinValue { get; }
        public double MaxValue { get; }

        public DistanceAttribute(double MinValue, double MaxValue)
        {
            this.MinValue = MinValue;
            this.MaxValue = MaxValue;
        }

        public string GetMinValueErrorMessage() => $"Distance must be greater than {MinValue}.";
        public string GetMaxValueErrorMessage() => $"Distance must be less than or equal to {MaxValue}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var site = (Site)validationContext.ObjectInstance;

            if (site.Distance.CompareTo(MinValue) < 0)
            {
                return new ValidationResult(GetMinValueErrorMessage());
            }

            if (site.Distance.CompareTo(MaxValue) > 0)
            {
                return new ValidationResult(GetMaxValueErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
