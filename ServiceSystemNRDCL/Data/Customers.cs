using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Data
{
    public class Customers
    {

        [Key]
        [Required(ErrorMessage = "Please Enter the CID!")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = "Invalid Input!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid CID!")]
        public string CustomerCID { get; set; }

        [Required(ErrorMessage = "Please enter First name!")]
        [StringLength(250, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last name!")]
        [StringLength(250, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter phone number!")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = "Invalid Input!")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Invalid phone number!")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter email!")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please enter password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }



    }
}
