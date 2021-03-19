using ServiceSystemNRDCL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public interface IOrderRepository
    {
        Task<CustomResponse> AddAsync(Order Order);
        Task<Order> FindByIDAsync(int id);
        Task<List<Order>> FindAllAsync(string userID, bool isAdmin);
        Task<List<Product>> FindAllProductAsync(string userID, bool isAdmin);
        Task<bool> IsIDExistsAsync(int id);
        Task<List<Product>> ProductDropdownListAsync();
        Task<CustomResponse> Calculate(string customerCID, int siteID, int productID, double quantity);
    }
}
