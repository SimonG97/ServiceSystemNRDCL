using ServiceSystemNRDCL.Models;
using System.ComponentModel.DataAnnotations;

namespace ServiceSystemNRDCL.Validations
{
    public class ProductNameAttribute : ValidationAttribute
    {
        public string ProductName { get; }

        public ProductNameAttribute(string ProductName)
        {
            this.ProductName = ProductName;
        }

        public string GetErrorMessage() => $"Product name length must be greater than or equal to {ProductName.Length}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var product = (Product)validationContext.ObjectInstance;

            if (product.ProductName != null && product.ProductName.Length < ProductName.Length)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
