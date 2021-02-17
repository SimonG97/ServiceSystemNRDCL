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
        private readonly CustomerRepository customerRepository = null;

        public CustomerController() {
            customerRepository = new CustomerRepository();
        }
        public IActionResult CheckCID()
        {
            return View();
        }
    }
}
