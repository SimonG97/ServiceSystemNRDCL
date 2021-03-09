﻿using Microsoft.AspNetCore.Identity;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        public AccountRepository(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        //adding new users to database using custom identity
        public async Task<IdentityResult> CreateUserAsync(CustomerModel customer)
        {
            var user = new ApplicationUser()
            {

                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.Phone,
                Id = customer.CustomerCID,
                UserName = customer.CustomerCID

            };
            return await _userManager.CreateAsync(user, customer.Password);
        }

        //checking if the cid is already registered
        public async Task<ApplicationUser> CheckCustomer(CustomerModel customer)
        {
            return await _userManager.FindByIdAsync(customer.CustomerCID);
            
        }

        //signing in method
        public async Task<SignInResult> PasswordSignInAsync(LogInModel logIn)
        {
            return await _signInManager.PasswordSignInAsync(logIn.CID, logIn.Password,logIn.RememberMe, false);
        }

        //log out method
        public async Task SignOutAsync() {
             await _signInManager.SignOutAsync();
        }

        //method to change the password
        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePassword) {
            var user = await _userManager.FindByIdAsync(_userService.GetUserId());
           return await _userManager.ChangePasswordAsync(user,changePassword.CurrentPassword,changePassword.NewPassword);
        }

    }
}
