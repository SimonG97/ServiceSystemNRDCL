using Microsoft.AspNetCore.Identity;
using ServiceSystemNRDCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ServiceSystemNRDCL.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AccountRepository(UserManager<IdentityUser> userManager) {
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateUserAsync(CustomerModel customer)
        {
            var user = new IdentityUser()
            {
                
                UserName = customer.CustomerName,
                Email = customer.Email,
                PhoneNumber = customer.Phone,
                Id=customer.CustomerCID,
                
            
            };
           return await _userManager.CreateAsync(user,customer.Password);
           
          

        }
    }
}
