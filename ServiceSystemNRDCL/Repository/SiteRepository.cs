using System.Collections.Generic;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ServiceSystemNRDCL.Repository
{
    public class SiteRepository : ISiteRepository
    {
        private readonly CustomerContext _context;
        private readonly IAccountRepository _accountRepository;

        public SiteRepository(CustomerContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        public async Task<bool> Add(Site site)
        {
            _context.Sites.Add(site);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Site site)
        {
            _context.Sites.Remove(site);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Site site)
        {
            _context.Sites.Update(site);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Site> FindByID(int? id)
        {
            return await _context.Sites.FirstOrDefaultAsync(m => m.SiteID == id);
        }

        public async Task<List<Site>> FindAll()
        {
            return await _context.Sites.ToListAsync();
        }

        public async Task<List<Site>> FindAll(string customerCID)
        {
            var siteList = await _context.Sites.ToListAsync();
            return siteList.Select(sites => new Site
            {
                SiteID = sites.SiteID,
                CustomerID = sites.CustomerID,
                SiteName = sites.SiteName,
                Distance = sites.Distance,
            }).Where(site => site.CustomerID == customerCID).ToList();
        }

        public async Task<bool> IsIDExists(int id)
        {
            return await _context.Sites.AnyAsync(e => e.SiteID == id);
        }
    }
}

