using Microsoft.AspNetCore.Mvc;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerRepository customerRepository;

        public CustomerController() {
            customerRepository = new CustomerRepository();
        }

        [Route("Site_Registration",Name ="SiteRegistration")]
        public IActionResult SiteRegistration()
        {
            return View();
        }


        [Route("Home-Page", Name = "HomePage")]
        public IActionResult HomePage()
        {
            return View();
        }

      
        public IActionResult AddNewCustomer( CustomerModel customer)
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckCustomer(CustomerModel customer)
        {
            return View();
        }

    }
}
