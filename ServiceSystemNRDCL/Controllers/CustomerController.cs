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
        private readonly CustomerRepository _customerRepository;

        public CustomerController(CustomerRepository customerRepository) {
            _customerRepository = customerRepository;
        }

        [Route("Site_Registration",Name ="SiteRegistration")]
        public ViewResult SiteRegistration()
        {
            ViewBag.status = "active font-weight-bold";
            return View();
        }


        [Route("Home-Page", Name = "HomePage")]
        public ViewResult HomePage()
        {
            
            return View();
        }

        [Route("Deposit-Advance", Name = "DespositAdvance")]
        public ViewResult DespositAdvance()
        {
           
            return View();
        }


      


        [HttpPost]
        public IActionResult CheckCustomer(CustomerModel customer)
        {

            return View("HomePage");
        }

    }
}
