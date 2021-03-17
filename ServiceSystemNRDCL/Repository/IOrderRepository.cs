using ServiceSystemNRDCL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public interface IOrderRepository
    {
        Task<CustomResponse> Add(Order Order);
        Task<bool> Remove(int id);
        Task<bool> Update(Order Order);
        Task<Order> FindByID(int id);
        Task<List<Order>> FindAll();
        Task<bool> IsIDExists(int id);
        Task<List<Product>> ProductDropdownList();
        Task<CustomResponse> Calculate(string customerCID, int siteID, int productID, double quantity);
    }
}
