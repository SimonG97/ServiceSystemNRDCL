using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ServiceSystemNRDCL.Controllers
{
    [Authorize(Roles ="Customer,Admin")]
    public class CustomerController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        public CustomerController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        

        //method to return view for home page.
        [Route("Home-Page", Name = "HomePage")]
        public ViewResult HomePage()
        {
            return View();
        }


        //method to return view for View Orders.
        [Route("View-Orders", Name = "ViewOrders")]
        public ViewResult ViewOrders()
        {
            return View();
        }

        //log out action method
        public async Task<IActionResult> LogOut()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("LogIn", "Home");
        }


        //change password method
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword)
        {
            if (ModelState.IsValid)
            {
                //changing the password from repository
                var result = await _accountRepository.ChangePasswordAsync(changePassword);
                if (!result.Succeeded)
                {
                    ViewBag.PassWordErrors = new List<string>();
                    ViewBag.CurrentPasswordError = new List<string>();
                    foreach (var errorMessage in result.Errors)
                    {
                        if (errorMessage.Description.Contains("Passwords"))
                        {
                            ViewBag.PasswordErrors.Add(errorMessage.Description);
                        }
                        else {
                            ViewBag.CurrentPasswordError.Add(errorMessage.Description);
                        }
                    }
                }
                ModelState.Clear();
                ViewBag.success = result.Succeeded;
            }
            return View();
        }

  



    }
}
