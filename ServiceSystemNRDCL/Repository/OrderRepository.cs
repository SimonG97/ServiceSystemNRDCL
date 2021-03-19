using System.Collections.Generic;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

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

        public async Task<List<Order>> FindAllAsync(string userID, bool isAdmin)
        {
            userID = isAdmin ? null : userID;
            List<ApplicationUser> applicationUserList = await _context.ApplicationUsers.ToListAsync();
            List<Order> orderList = _context.Orders.ToListAsync().Result.
                Select(data => new Order
                {
                    CustomerCID = data.CustomerCID,
                    PriceAmount = data.PriceAmount,
                    TansportAmount = data.TansportAmount,
                    AdvanceBalance = data.AdvanceBalance
                }).Where(data => data.CustomerCID == userID || userID == null).ToList();

            List<Deposit> depositList = _depositRepository.FindAll().Result.
                 Select(data => new Deposit
                 {
                     CustomerID = data.CustomerID,
                     Balance = data.Balance,
                 }).Where(data => data.CustomerID == userID || userID == null).ToList();

            var orderedList = from order in orderList
                              join user in applicationUserList on order.CustomerCID equals user.Id
                              join deposit in depositList on order.CustomerCID equals deposit.CustomerID
                              select new Order
                              {
                                  CustomerCID = order.CustomerCID,
                                  CustomerName = user.FirstName + " " + user.LastName,
                                  PriceAmount = order.PriceAmount,
                                  TansportAmount = order.TansportAmount,
                                  AdvanceBalance = deposit.Balance
                              };
            return orderedList.GroupBy(data => data.CustomerCID)
                .Select(order => new Order
                {
                    CustomerCID = order.First().CustomerCID,
                    CustomerName = order.First().CustomerName,
                    PriceAmount = order.Sum(data => data.PriceAmount),
                    TansportAmount = order.Sum(data => data.TansportAmount),
                    AdvanceBalance = order.First().AdvanceBalance,
                }).ToList();
        }

        public async Task<List<Product>> FindAllProductAsync(string userID, bool isAdmin)
        {
            userID = isAdmin ? null : userID;
            List<Order> orderList = _context.Orders.ToListAsync().Result.
                Select(data => new Order
                {
                    CustomerCID = data.CustomerCID,
                    ProductID = data.ProductID,
                    PriceAmount = data.PriceAmount,
                    TansportAmount = data.TansportAmount
                }).Where(data => data.CustomerCID == userID || userID == null).ToList();

            var productList = await _productRepository.FindAll();
            var productOrderedList = from order in orderList
                                     join product in productList on order.ProductID equals product.ProductID
                                     select new Product
                                     {
                                         ProductID = product.ProductID,
                                         ProductName = product.ProductName,
                                         PriceAmount = order.PriceAmount,
                                         TransportAmount = order.TansportAmount
                                     };
            return productOrderedList.GroupBy(data => data.ProductID)
                .Select(product => new Product
                {
                    ProductID = product.First().ProductID,
                    ProductName = product.First().ProductName,
                    PriceAmount = product.Sum(data => data.PriceAmount),
                    TransportAmount = product.Sum(data => data.TransportAmount)
                }).ToList();
        }

        public async Task<Order> FindByIDAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(m => m.OrderID == id);
        }

        public async Task<CustomResponse> AddAsync(Order order)
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

            _context.Orders.Add(orderDetails);
            await _context.SaveChangesAsync();

            // To update the deposit balance of given customer.
            deposit.DepositID = 1;
            deposit.Balance = deposit.Balance - orderDetails.OrderedAmount;
            deposit.LastAmount = -orderDetails.OrderedAmount;
            await _depositRepository.Update(deposit);
            return response;
        }

        public async Task<bool> IsIDExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(e => e.OrderID == id);
        }

        public async Task<List<Product>> ProductDropdownListAsync()
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
                message += $"{"Total Order Amount"}: Nu. {orderedAmount} ";
                message += $"{" Advance Balance"}: Nu. {deposit.Balance} ";
                message += $"{" Required Amount"}: Nu. {orderedAmount - deposit.Balance}";

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
