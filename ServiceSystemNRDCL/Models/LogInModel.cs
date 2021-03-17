using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Models
{
    public class LogInModel
    {
        [Required(ErrorMessage ="Please enter CID!")]
        [Display(Name ="Customer CID")]
        public string CID { get; set; }

        [Required(ErrorMessage ="Please enter password!")]
        [DataType(DataType.Password)]
        public string Password{ get; set; }

        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }
        public string Email { get; set; }
    }
}
