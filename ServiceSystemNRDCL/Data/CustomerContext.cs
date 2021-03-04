using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceSystemNRDCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ServiceSystemNRDCL.Data
{
    public class CustomerContext:IdentityDbContext<ApplicationUser>
    {
        public CustomerContext(DbContextOptions<CustomerContext> options): base(options)
        {
        }

        public DbSet<Customers> Customers { get; set; }
    }
}
