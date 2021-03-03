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


        private readonly IAccountRepository _accountRepository;
       //dependency injection of customer repository
        public HomeController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        //view method for index page
        public IActionResult Index(bool message=true)
        {
            ViewBag.message = message;
            return View();
        }

        //view methood to return the view to register customer
        public ViewResult RegisterCustomer(bool success = false, bool present=false)
        {
            ViewBag.present = present;
            ViewBag.success = success;
            return View();
        }

        //post method to register a customer
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(CustomerModel customer)
        {
            // checking if the model is valid
            if (ModelState.IsValid)
            {
                //checking if the customer is already registered.
                var result = await _accountRepository.CreateUserAsync(customer);
                if (!result.Succeeded) {
                    foreach (var errorMessage in result.Errors ) {
                        ModelState.AddModelError("",errorMessage.Description);
                    }
                    ModelState.Clear();
                }
                ViewBag.success = result.Succeeded;
                
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
