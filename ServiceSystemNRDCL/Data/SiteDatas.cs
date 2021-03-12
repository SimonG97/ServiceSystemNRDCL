using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Data
{
    public class SiteDatas
    {
        [Key]
        public int Id { get; set; }
        public string CustomerCID { get; set; }


    }
}
