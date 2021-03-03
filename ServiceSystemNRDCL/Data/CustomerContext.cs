using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ServiceSystemNRDCL.Data
{
    public class CustomerContext:IdentityDbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options): base(options)
        {
        }

        public DbSet<Customers> Customers { get; set; }
    }
}
