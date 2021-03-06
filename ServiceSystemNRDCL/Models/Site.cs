﻿using Microsoft.AspNetCore.Mvc;
using ServiceSystemNRDCL.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSystemNRDCL.Models
{
    public class Site
    {
        [Key]
        [Display(Name = "ID")]
        public int SiteID { get; set; }

        [Required(ErrorMessage = "Customer CID is mandatory.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Customer CID must have a minimum and maximum length of 11.")]
        [Display(Name = "Customer CID")]
        [Remote(action: "VerifyCustomerCID", controller: "Site")]
        public string CustomerID { get; set; }

        [Required(ErrorMessage = "Site name is mandatory.")]
        //[StringLength(100, MinimumLength = 3)]
        [Display(Name = "Site Name")]
        [Remote(action: "VerifySiteName", controller: "Site")]
        public string SiteName { get; set; }

        [Required(ErrorMessage = "Distance is mandatory.")]
        [RegularExpression("^(0*[1-9][0-9]*(\\.[0-9]+)?|0+\\.[0-9]*[1-9][0-9]*)$", ErrorMessage = "Please enter a valid distance.")]
        [Display(Name = "Distance")]
        [Distance(0, 2000)]
        public double Distance { get; set; }

        [NotMapped]
        public string Text { get; set; }

        public Site() { }

        public Site(string CustomerID)
        {
            this.CustomerID = CustomerID;
        }
    }
}
