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

        [Required(ErrorMessage = "Please enter phone number!")]
        [Display(Name = "Mobile Number")]
        public int? Phone { get; set; }

        [Required(ErrorMessage = "Please enter email!")]
        [EmailAddress]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Please enter password!")]
        [Display(Name = " Set Password")]
        [DataType(DataType.Password)]
        public string Password{ get; set; }

        
    }
}
