using System.Collections.Generic;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CustomerContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ISiteRepository _siteRepository;
        private readonly IDepositRepository _depositRepository;

        public OrderRepository(CustomerContext context, IProductRepository productRepository,
            ISiteRepository siteRepository, IDepositRepository depositRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _siteRepository = siteRepository;
            _depositRepository = depositRepository;
        }

        public async Task<List<Order>> FindAll()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> FindByID(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(m => m.OrderID == id);
        }

        public async Task<CustomResponse> Add(Order order)
        {
            var deposit = await _depositRepository.FindByID(order.CustomerCID);
            var product = _productRepository.FindByID(order.ProductID).Result;
            var site = _siteRepository.FindByID(order.SiteID).Result;
            var response = CalculateData(order.CustomerCID, product, site, deposit, order.Quantity);

            if (response.Status != 1)
            {
                return response;
            }

            var orderDetails = (Order)response.ResponseData;

            //_context.Orders.Add(orderDetails);
            //await _context.SaveChangesAsync();

            // To update the deposit balance of given customer.
            deposit.Balance = orderDetails.AdvanceBalance;
            deposit.LastAmount = -orderDetails.OrderedAmount;
            //await _depositRepository.Update(deposit);
            return response;
        }

        public async Task<bool> Remove(int id)
        {
            _context.Orders.Remove(await FindByID(id));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsIDExists(int id)
        {
            return await _context.Orders.AnyAsync(e => e.OrderID == id);
        }

        public async Task<List<Product>> ProductDropdownList()
        {
            return await _productRepository.FindAll();
        }

        public async Task<CustomResponse> Calculate(string customerCID, int siteID, int productID, double quantity)
        {
            var product = await _productRepository.FindByID(productID);
            var site = await _siteRepository.FindByID(siteID);
            var deposit = await _depositRepository.FindByID(customerCID);

            return CalculateData(customerCID, product, site, deposit, quantity);
        }

        private CustomResponse CalculateData(string customerCID, Product product, Site site, Deposit deposit, double quantity)
        {
            var message = "";

            double priceAmount = quantity * product.Price;
            double transportAmount = product.Rate * quantity * site.Distance;
            double orderedAmount = priceAmount + transportAmount;
            double advanceBalance = deposit.Balance - orderedAmount;

            // To validate if the customer have required balance amount to place order.
            if (orderedAmount > deposit.Balance)
            {
                message += $"<table><tr><td>{"Total Order Amount"}:</td><td>  Nu. {orderedAmount}</td></tr>";
                message += $"<tr><td>{"Advance Balance"}:</td><td>  Nu. {deposit.Balance}</td></tr>";
                message += $"<tr><td>{"Required Amount"}:</td><td>  Nu. {orderedAmount - deposit.Balance}</td></tr><table>";

                var orders = new Order()
                {
                    CustomerCID = customerCID,
                    ProductID = product.ProductID,
                    SiteID = site.SiteID,
                    Quantity = quantity,
                };

                return new CustomResponse(0, message, orders);
            }

            // To set the total price amount, transport amount and advance balance.
            var order = new Order()
            {
                CustomerCID = customerCID,
                ProductID = product.ProductID,
                SiteID = site.SiteID,
                PriceAmount = priceAmount,
                TansportAmount = transportAmount,
                AdvanceBalance = advanceBalance,
                OrderedAmount = orderedAmount
            };

            return new CustomResponse(1, message, order);
        }
    }
}
