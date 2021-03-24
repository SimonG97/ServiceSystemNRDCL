using ServiceSystemNRDCL.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Areas.Admin.Models
{
    public class ProductModel: IValidatableObject
    {

        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is mandatory.")]
        [StringLength(250, MinimumLength = 3)]
        [Display(Name = "Product Name")]
        [ProductName("some length")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is mandatory.")]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Rate is mandatory.")]
        [Display(Name = "Rate")]
        public double Rate { get; set; }
        public string Text { get; set; }

        public ProductModel() { }

        public ProductModel(string ProductName, double Price, double Rate)
        {
            this.ProductName = ProductName;
            this.Price = Price;
            this.Rate = Rate;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Price > 10000)
            {
                yield return new ValidationResult($"Price must be less than or equal to 100.", new[] { nameof(Price) });
            }

            if (Rate > 100)
            {
                yield return new ValidationResult($"Rate must be less than or equal to 100.", new[] { nameof(Rate) });
            }
        }
    }
}
