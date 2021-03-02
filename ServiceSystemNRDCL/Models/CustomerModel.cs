using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Models
{
    public class CustomerModel
    {
        [Key]
        [Required(ErrorMessage = "Please Enter the CID")]
        [Display(Name = "Customer CID")]
        public int? CustomerCID { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [StringLength(250, MinimumLength = 3)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Phone number is mandatory")]
        [Display(Name = "Mobile Number")]
        public int? Phone { get; set; }

        [Required(ErrorMessage = "Email Address is mandatory")]
        [EmailAddress]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Password is mandatory")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password{ get; set; }

        
    }
}
