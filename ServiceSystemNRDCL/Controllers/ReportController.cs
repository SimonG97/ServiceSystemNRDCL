using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            string userID = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            List<Order> orderList = await _orderRepository.FindAllAsync(userID, isAdmin);
            ViewData["ProductList"] = await _orderRepository.FindAllProductAsync(userID, isAdmin);
            return View(orderList);
        }
    }
}
