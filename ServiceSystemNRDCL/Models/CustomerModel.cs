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
        [Required(ErrorMessage = "Customer CID is mandatory.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Customer CID must have a minimum and maximum length of 11.")]
        [Display(Name = "Customer CID")]
        public string CustomerCID { get; set; }

        [Required(ErrorMessage = "Customer Name is mandatory.")]
        [StringLength(250, MinimumLength = 3)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Phone number is mandatory")]
        [RegularExpression("[0-9]{8,8}", ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(8, MinimumLength = 8)]
        [Display(Name = "Mobile Number")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email Address is mandatory")]
        [EmailAddress]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Password is mandatory")]
        [StringLength(6, MinimumLength = 6)]
        [Display(Name = " Set Password")]
        [DataType(DataType.Password)]
        public string Password{ get; set; }


    }
}
