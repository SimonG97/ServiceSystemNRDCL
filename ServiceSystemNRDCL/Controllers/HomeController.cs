using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;
using ServiceSystemNRDCL.Service;
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
        private readonly IUserService _userService;
        
       //dependency injection of customer repository and user services
        public HomeController(IAccountRepository accountRepository,
            IUserService userService)
        {
            _accountRepository = accountRepository;
            _userService = userService;
            
        }

        //view method for Log in page
        
        [HttpGet]
        public IActionResult LogIn(string returnUrl)
        {

            //checking if the user is authenticated
             if (_userService.IsAuthenticated()) {
                if (_userService.IsAdmin()) {
                    return RedirectToAction("Index", "Product");
                }
                else {
                    return RedirectToAction("HomePage", "Customer");
                }
            }
            //checking ig there is return url
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.message = true;
            }
            return View(returnUrl);
        }

        //method to log in as customer
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInModel logIn,string returnUrl)
        {
            if (ModelState.IsValid) {
                //passing log in model to account repository
               var result= await _accountRepository.PasswordSignInAsync(logIn);
                //checking if the log in succesful
                if (result.Succeeded) {
                    //redirecting to admin page if the user is a admin
                    if (_userService.IsAdmin())
                    {
                        return RedirectToAction("Index","Product");
                    }
                    //checking if there is return URl
                    else if (!string.IsNullOrEmpty(returnUrl)) {
                       //redirecting the url below if there is ReturnURL
                        return LocalRedirect(returnUrl);
                    }
                    //redirecting to below URL if there is no return URL
                    else {
                        return RedirectToAction("HomePage", "Customer");
                    }
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "You have to confirm email to log in");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                }
            }
            return View();
        }


        //view methood to return the view to register customer
        public ViewResult Register(bool success = false, bool present=false)
        {
            ViewBag.present = present;
            ViewBag.success = success;
     
            return View();
        }

        //post method to register a customer
        [HttpPost]
        public async Task<IActionResult> Register(CustomerModel customer)
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
                    return RedirectToAction("ConfirmEmail",new {email=customer.Email});
                    
                }
                else { 
                    ViewBag.present=true;
                }
            }
            return View();
        }

        //method to show that user has confirmed email
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            LogInModel model = new LogInModel
            {
                Email = email
            };
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(" ", "+");
                var result = await _accountRepository.ConfirmEmailAsync(uid,token);
                if (result.Succeeded) {
                    ViewBag.EmailVerified = true;
                    return View("LogIn");
                }
            }
            ViewBag.Registered = true;
            return View("Register");

        }

        //method to resend the confirmation mail
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(LogInModel model)
        {
            var user = await _accountRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                //checking if the user has already confirmed the email
                if (user.EmailConfirmed)
                {
                    ViewBag.EmailVerified = true;
                    return View("LogIn");
                }
                await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                ViewBag.Registered = true;
                ViewBag.EmailSent = true;
                ModelState.Clear();
                
            }
            else {
                ModelState.AddModelError("", "Something went wrong.");
            }
            return View("Register");

        }

        [HttpGet("fogot-password")]
        //method for forgot password
        public IActionResult ForgotPassword() {
            return View();
        }

        [HttpPost("fogot-password")]
        //method for forgot password
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            if (ModelState.IsValid) {
                var user =await _accountRepository.GetUserByEmailAsync(forgotPassword.Email);
                if (user != null)
                {
                    await _accountRepository.GenerateForgotPasswordTokenAsync(user);
                    ModelState.Clear();
                    forgotPassword.EmailSent = true;
                }
                else {
                    ModelState.AddModelError("Email","Please enter a registered email id!");
                }
            }
            return View(forgotPassword);
        }

        [HttpGet("reset-password")]
        //method for resetting password after user has forgotten the password
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel
            {
                Token=token,
                UserId=uid
            };
            return View(resetPasswordModel);
        }

        //post method for reset password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPassword)
        {
            if(ModelState.IsValid){
                resetPassword.Token = resetPassword.Token.Replace(" ", "+");
                var result =await _accountRepository.ResetPasswordAsync(resetPassword);
                if (result.Succeeded) {
                    ModelState.Clear();
                    resetPassword.IsSuccess = true;
                    return View(resetPassword);
                }
                ViewBag.PassWordErrors = new List<string>();
                foreach (var errorMessage in result.Errors)
                {
                    if (errorMessage.Description.Contains("Passwords"))
                    {
                        ViewBag.PasswordErrors.Add(errorMessage.Description);
                    }
                    
                }


            }
            return View(resetPassword);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
