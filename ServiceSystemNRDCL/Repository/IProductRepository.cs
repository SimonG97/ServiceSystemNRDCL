using ServiceSystemNRDCL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public interface IProductRepository
    {
        Task<bool> Add(Product Product);
        Task<bool> Remove(int id);
        Task<bool> Update(Product Product);
        Task<Product> FindByID(int? id);
        Task<List<Product>> FindAll();
        Task<bool> IsIDExists(int id);
    }
}
