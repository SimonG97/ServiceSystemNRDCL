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
        [Required(ErrorMessage = "Please Enter the CID!")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = "Invalid Input!")]
        [StringLength(11,MinimumLength =11, ErrorMessage ="Invalid CID!")]
        [Display(Name = "Customer CID")]
        public string CustomerCID { get; set; }

        [Required(ErrorMessage = "Please enter name!")]
        [StringLength(250, MinimumLength = 3)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please enter phone number!")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = "Invalid Input!")]
        [StringLength(8,MinimumLength =8,ErrorMessage ="Invalid phone number!")]
        [Display(Name = "Mobile Number")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter email!")]
        [EmailAddress]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Please enter password!")]
        [DataType(DataType.Password)]
        [Display(Name ="Set Password")]
        public string Password{ get; set; }

        [Required(ErrorMessage = "Confirm Password is Required!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password do not match")]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }


    }
}
