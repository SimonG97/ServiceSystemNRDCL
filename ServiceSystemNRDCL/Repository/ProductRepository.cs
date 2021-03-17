using System.Collections.Generic;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ServiceSystemNRDCL.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CustomerContext _context;

        public ProductRepository(CustomerContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> FindAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> FindByID(int? id)
        {
            return await _context.Products.FirstOrDefaultAsync(m => m.ProductID == id);
        }

        public async Task<bool> Add(Product Product)
        {
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// To remove the product based on the product ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Task<bool></returns>
        public async Task<bool> Remove(int id)
        {
            var product = FindByID(id).Result;

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> Update(Product Product)
        {
            _context.Products.Update(Product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsIDExists(int id)
        {
            return await _context.Products.AnyAsync(e => e.ProductID == id);
        }
    }
}
