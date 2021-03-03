using Microsoft.AspNetCore.Identity;
using ServiceSystemNRDCL.Models;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(CustomerModel customer);
    }
}