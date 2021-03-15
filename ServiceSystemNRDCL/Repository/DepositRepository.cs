﻿using System.Collections.Generic;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ServiceSystemNRDCL.Repository
{
    public class DepositRepository : IDepositRepository
    {
        private readonly CustomerContext _context;

        public DepositRepository(CustomerContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Deposit deposit)
        {
            _context.Deposits.Add(deposit);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Deposit deposit)
        {
            _context.Deposits.Remove(deposit);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Deposit deposit)
        {
            _context.Deposits.Update(deposit);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Deposit> FindByID(string customerCID)
        {
            return await _context.Deposits.FirstOrDefaultAsync(m => m.CustomerID == customerCID);
        }

        public async Task<List<Deposit>> FindAll()
        {
            return await _context.Deposits.ToListAsync();
        }

        public async Task<List<Deposit>> FindAll(string customerCID)
        {
            var depositList = await _context.Deposits.ToListAsync();
            return depositList.Select(deposit => new Deposit
            {
                CustomerID = deposit.CustomerID,
                Balance = deposit.Balance,
                LastAmount = deposit.LastAmount,
            }).Where(deposits => deposits.CustomerID == customerCID).ToList();
        }

        public async Task<bool> IsIDExists(string customerCID)
        {
            return await _context.Deposits.AnyAsync(e => e.CustomerID == customerCID);
        }
    }
}
