using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly CustomerRepository _customerRepository;

       
        public HomeController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        public IActionResult Index()
        {
            return View();
        }

        public ViewResult RegisterCustomer(bool success = false, bool present=false)
        {
            ViewBag.present = present;
            ViewBag.success = success;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                if (await _customerRepository.CheckCustomer(customer))
                {
                    return RedirectToAction(nameof(RegisterCustomer), new { success = false, present = true });
                }
                _customerRepository.AddNewCustomer(customer);
                return RedirectToAction(nameof(RegisterCustomer), new { success = true, present = false });
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
