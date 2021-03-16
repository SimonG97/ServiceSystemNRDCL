using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
     
        public AccountRepository(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IUserService userService, IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
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
            var result= await _userManager.CreateAsync(user, customer.Password); 
            if (result.Succeeded) {
                await GenerateEmailConfirmationTokenAsync(user);
            }
            return result;
        }
        //method to get user by email
        public async Task<ApplicationUser> GetUserByEmailAsync(string email) {
            return await _userManager.FindByEmailAsync(email);
        }
        //method to send confirmation email 
        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user) {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailComfirmationEmail(user, token);
            }
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

        //method to confirm email 
        public async Task <IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }

        //Method to send the confirmation mail to the users
        private async Task SendEmailComfirmationEmail(ApplicationUser user,string token) {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolder = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}",user.FirstName),
                    new KeyValuePair<string, string>("{{link}}",string.Format(appDomain+confirmationLink,user.Id,token))
                }

            };
            await _emailService.SendEmailConfirmation(options);

        }

    }
}
