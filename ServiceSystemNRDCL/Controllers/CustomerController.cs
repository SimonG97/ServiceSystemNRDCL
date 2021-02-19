using Microsoft.AspNetCore.Mvc;
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
    }
}
