using ServiceSystemNRDCL.Data;
using ServiceSystemNRDCL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public class CustomerRepository
    {
        private readonly CustomerContext _context;
        public CustomerRepository(CustomerContext context) {
            _context = context;
        }
        public async void AddNewCustomer(CustomerModel customer) {
            var newCustomer = new Customers()
            {
                CustomerCID = customer.CustomerCID,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                Phone = customer.Phone,
                Password = customer.Password
            };
            await _context.Customers.AddAsync(newCustomer);
            await _context.SaveChangesAsync();

        }
        public async Task<bool> CheckCustomer(CustomerModel customer){
            var allCustomers = await _context.Customers.ToListAsync();
            if (allCustomers.Any()) {
                return allCustomers.Exists(p=>p.CustomerCID==customer.CustomerCID);
            }
            return false;
        }
    }
}
