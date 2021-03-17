using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSystemNRDCL.Models
{
    public class Deposit : IValidatableObject
    {

        [Key]
        [Display(Name = "Customer CID")]
        public string CustomerID { get; set; }

        [Display(Name = "Last Amount")]
        public double LastAmount { get; set; }

        [Display(Name = "Balance")]
        public double Balance { get; set; }

        [NotMapped]
        public int DepositID { get; set; }

        public Deposit() { }

        public Deposit(string CustomerID, int DepositID)
        {
            this.CustomerID = CustomerID;
            this.DepositID = DepositID;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Balance <= 0)
            {
                yield return new ValidationResult($"Balance must be greater than 0.", new[] { nameof(Balance) });
            }
        }
    }

}
