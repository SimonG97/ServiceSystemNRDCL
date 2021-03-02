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

        //dependency injection of customer repository.
        public CustomerController(CustomerRepository customerRepository) {
            _customerRepository = customerRepository;
        }

        //method to return view for site registration.
        [Route("Site_Registration",Name ="SiteRegistration")]
        public ViewResult SiteRegistration()
        {
            ViewBag.status = "active font-weight-bold";
            return View();
        }

        //method to return view for home page.
        [Route("Home-Page", Name = "HomePage")]
        public ViewResult HomePage()
        {
            
            return View();
        }

        //method to return view for deposit advance.
        [Route("Deposit-Advance", Name = "DespositAdvance")]
        public ViewResult DespositAdvance()
        {
           
            return View();
        }

        //method to return view for deposit advance.
        [Route("Place-Order", Name = "Place Order")]
        public ViewResult PlaceOrder()
        {

            return View();
        }

        //method to return view for deposit advance.
        [Route("View-Orders", Name = "ViewOrders")]
        public ViewResult ViewOrders()
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
