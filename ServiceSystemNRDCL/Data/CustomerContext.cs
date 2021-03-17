using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceSystemNRDCL.Models;


namespace ServiceSystemNRDCL.Data
{
    public class CustomerContext:IdentityDbContext<ApplicationUser>
    {
        public CustomerContext(DbContextOptions<CustomerContext> options): base(options)
        {
        }

        public DbSet<CustomerOrders> CustomerOrders { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
