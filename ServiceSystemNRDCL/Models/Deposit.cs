using System.ComponentModel.DataAnnotations;

namespace ServiceSystemNRDCL.Models
{
    public class Deposit
    {

        [Key]
        public string CustomerID { get; set; }
        public decimal LastAmount { get; set; }
        public decimal Balance { get; set; }
    }

}
