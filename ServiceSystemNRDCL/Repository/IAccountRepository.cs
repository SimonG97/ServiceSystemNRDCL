using Microsoft.AspNetCore.Identity;
using ServiceSystemNRDCL.Models;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public interface IAccountRepository
    {
        Task<ApplicationUser> CheckCustomer(CustomerModel customer);
        Task<IdentityResult> CreateUserAsync(CustomerModel customer);
        Task<SignInResult> PasswordSignInAsync(LogInModel logIn);
        Task SignOutAsync();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePassword);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
    }
}