using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSystemNRDCL.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Customer is mandatory."), Display(Name = "Customer CID")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "{0} must have a minimum and maximum length of {1}.")]
        public string CustomerCID { get; set; }

        [Display(Name = "Site"), Required(ErrorMessage = "Site is mandatory.")]
        public int SiteID { get; set; }

        [Display(Name = "Product"), Required(ErrorMessage = "Product is mandatory.")]
        public int ProductID { get; set; }

        [Display(Name = "Price Amount")]
        public double PriceAmount { get; set; }

        [Display(Name = "Tansport Amount")]
        public double TansportAmount { get; set; }

        [Display(Name = "Advance Balance")]
        public double AdvanceBalance { get; set; }

        [Display(Name = "Quantity"), Required(ErrorMessage = "Quantity is mandatory.")]
        public double Quantity { get; set; }

        [NotMapped, Display(Name = "Tansport Amount")]
        public double OrderedAmount { get; set; }
    }
}
