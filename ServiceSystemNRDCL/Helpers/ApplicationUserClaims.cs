using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ServiceSystemNRDCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Helpers
{
    public class ApplicationUserClaims : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaims(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,IOptions<IdentityOptions> options)
            :base(userManager,roleManager,options)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity= await base.GenerateClaimsAsync(user);
            //adding a claim to a user
            identity.AddClaim(new Claim("FirstName",user.FirstName ?? ""));
            return identity;
        }

    }
}
