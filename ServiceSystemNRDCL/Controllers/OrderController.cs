using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ServiceSystemNRDCL.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISiteRepository _siteRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager,
            ISiteRepository siteRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
            _siteRepository = siteRepository;
            _productRepository = productRepository;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? status, int? siteID, int? productID, double? quantity, string customerID)
        {
            var userID = _userManager.GetUserId(User);
            userID = string.IsNullOrEmpty(customerID) ? userID : customerID;
            var order = new Order();
            ViewBag.Status = false;
            ViewBag.Quantity = "";

            if (status != null && status > 0)
            {
                ViewBag.Status = true;
                ViewBag.Quantity = quantity.ToString();
                order = new Order() { CustomerCID = userID, SiteID = (int)siteID, ProductID = (int)productID };
            }

            ViewBag.SiteDropdownList = await GetSiteDropdownlist(userID);
            ViewBag.ProductDropdownList = await GetProductDropdownlist();
            ViewBag.CustomerID = userID;
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Calculate(string customerCID, int siteID, int productID, double quantity)
        {
            var result = await _orderRepository.Calculate(customerCID, siteID, productID, quantity);
            return new JsonResult(result);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] Order order)
        {
            var userID = _userManager.GetUserId(User);
            var data = new
            {
                status = 1,
                siteID = order.SiteID,
                productID = order.ProductID,
                quantity = order.Quantity,
                customerID = order.CustomerCID,
                message = "",
            };

            if (ModelState.IsValid)
            {
                CustomResponse response = await _orderRepository.AddAsync(order);
                if (response.Status == 1)
                {
                    TempData["SuccessMessage"] = "Order placed successfylly.";
                    return RedirectToAction(nameof(Index), new { status = 0, customerID = "" });
                }
                TempData["ResponseMessage"] = response.Message;
                return RedirectToAction(nameof(Index), data);
            }
            return RedirectToAction(nameof(Index), data);
        }

        public async Task<IActionResult> GetSiteDropdownList(string customerCID)
        {
            List<SelectListItem> siteList = new List<SelectListItem>();
            if (string.IsNullOrEmpty(customerCID))
            {
                return Json(siteList);
            }

            siteList = await GetSiteDropdownlist(customerCID);
            return Json(siteList);
        }

        /// <summary>
        /// To get the site dropdown list
        /// </summary>
        /// <param name="userID">current user ID</param>
        /// <returns>List of sites</returns>
        private async Task<List<SelectListItem>> GetSiteDropdownlist(string userID)
        {
            List<SelectListItem> siteList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "--- Please Select ---", Selected = true }
            };
            siteList.AddRange((await _siteRepository.FindAll(userID)).Select(site => new SelectListItem
            {
                Value = site.SiteID.ToString(),
                Text = site.SiteName,
                Selected = false
            }).ToList());

            return siteList;
        }

        /// <summary>
        /// To get the product dropdown list
        /// </summary>
        /// <returns>List of Products</returns>
        private async Task<List<SelectListItem>> GetProductDropdownlist()
        {
            List<SelectListItem> ProductList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "--- Please Select ---", Selected = true }
            };

            ProductList.AddRange((await _productRepository.FindAll()).Select(product => new SelectListItem
            {
                Value = product.ProductID.ToString(),
                Text = product.ProductName,
                Selected = false
            }).ToList());

            return ProductList;
        }
    }
}
