using ServiceSystemNRDCL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public interface ISiteRepository
    {
        Task<bool> Add(Site site);
        Task<bool> Remove(Site site);
        Task<bool> Update(Site site);
        Task<Site> FindByID(int id);
        Task<List<Site>> FindAll();
        Task<List<Site>> FindAll(string customerCID);
        Task<bool> IsIDExists(int id);
    }
}
