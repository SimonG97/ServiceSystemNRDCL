using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        public UserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        //method to get user id
        public string GetUserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        //method to check if user is logged in
        public bool IsAuthenticated() {
            return _httpContext.HttpContext.User.Identity.IsAuthenticated;
        }

        //method to check the user role
        public bool IsAdmin() {
            return _httpContext.HttpContext.User.IsInRole("Admin");
        }
    }
}
