using ServiceSystemNRDCL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public interface IDepositRepository
    {
        Task<bool> Add(Deposit deposit);
        Task<bool> Remove(Deposit deposit);
        Task<bool> Update(Deposit deposit);
        Task<Deposit> FindByID(string id);
        Task<List<Deposit>> FindAll();
        Task<List<Deposit>> FindAll(string customerCID);
        Task<bool> IsIDExists(string id);
    }
}
