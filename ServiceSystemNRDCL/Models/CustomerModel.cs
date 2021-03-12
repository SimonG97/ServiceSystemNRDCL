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
        [RegularExpression(@"([0-9]+)", ErrorMessage = "Invalid CID!")]
        [StringLength(11,MinimumLength =11, ErrorMessage ="Enter a valid CID!")]
        [Display(Name = "Customer CID")]
        public string CustomerCID { get; set; }

        [Required(ErrorMessage = "Please enter First name!")]
        [StringLength(250, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(250, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter phone number!")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = "Invalid phone number!")]
        [StringLength(8,MinimumLength =8,ErrorMessage ="Invalid phone number!")]
        [Display(Name = "Mobile Number")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter email!")]
        [EmailAddress(ErrorMessage ="Please enter a valid mail")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Please enter password!")]
        [Display(Name ="Set Password")]
        [DataType(DataType.Password)]
        public string Password{ get; set; }

        [Required(ErrorMessage = "Confirm Password is Required!")] 
        [Compare("Password", ErrorMessage = "The password do not match")]
        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }
}
