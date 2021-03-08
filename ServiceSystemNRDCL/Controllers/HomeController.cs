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

        //view method for Log in page
        public IActionResult LogIn(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.message = true;
               
            }
            return View();
        }

        //method to log in as customer
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInModel logIn, string message, string returnUrl)
        {
            if (ModelState.IsValid) {
                //passing log in model to account repository
               var result= await _accountRepository.PasswordSignInAsync(logIn);
                //checking if the log in succesful
                if (result.Succeeded) {
                    //checking if there is return URl
                    if (!string.IsNullOrEmpty(returnUrl)) {
                       //redirecting the url below if there is ReturnURL
                        return LocalRedirect(returnUrl);
                    }
                    //redirecting to below URL if there is no return URL
                    return RedirectToAction("HomePage","Customer");
                }
                ModelState.AddModelError("","Invalid Credentials");
            }
            return View();
        }


        //view methood to return the view to register customer
        public ViewResult RegisterCustomer(bool success = false, bool present=false)
        {
            ViewBag.present = present;
            ViewBag.success = success;
            ViewBag.ErrorMessage = false;
            return View();
        }

        //post method to register a customer
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(CustomerModel customer)
        {
            // checking if the model is valid
            if (ModelState.IsValid)
            {
                //checking if the cid is registered
                var user = await _accountRepository.CheckCustomer(customer);
                if (user == null)
                {
                    //adding the user in the database
                    var result = await _accountRepository.CreateUserAsync(customer);
                    //checking if it was successful
                    if (!result.Succeeded)
                    {
                        ViewBag.PassWordErrors = new List<string>();
                        ViewBag.EmailErrors = new List<string>();
                        foreach (var errorMessage in result.Errors)
                        {
                            if (errorMessage.Description.Contains("Passwords"))
                            {
                                ViewBag.PasswordErrors.Add(errorMessage.Description);
                            }
                            else {
                                ViewBag.EmailErrors.Add(errorMessage.Description);
                            }
           
                        }
                        
                        return View(customer);
                    }
                    ModelState.Clear();
                    ViewBag.success = result.Succeeded;
                    ViewBag.present = false;
                }
                else { 
                    ViewBag.present=true;
                }
            }
            return View();
        }

        //log out action method
        public async Task<IActionResult> LogOut() {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("LogIn", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
