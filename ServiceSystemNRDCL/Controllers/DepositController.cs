using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;

namespace ServiceSystemNRDCL.Controllers
{
    [Authorize]
    public class DepositController : Controller
    {
        private readonly IDepositRepository _depositRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public DepositController(IDepositRepository depositRepository, UserManager<ApplicationUser> userManager)
        {
            _depositRepository = depositRepository;
            _userManager = userManager;
        }

        // GET: Sites
        public async Task<IActionResult> Index(int? id, int? status)
        {
            var userID = _userManager.GetUserId(User);
            Deposit deposit = null;
            if (id != null && id > 0)
            {
                deposit = await _depositRepository.FindByID(userID);
                deposit.DepositID = 1;
            }

            if (status != null && status > 0)
            {
                ViewBag.Status = true;
                ViewBag.Message = status == 1 ? "created" : "updated";
            }

            Deposit depositDetails = await _depositRepository.FindByID(userID);
            ViewData["DepositList"] = await _depositRepository.FindAll(userID); ;
            ViewBag.CustomerID = userID;
            ViewBag.LastAmount = depositDetails == null ? 0 : depositDetails.LastAmount;
            return View(deposit);
        }

        // POST: Sites/Create
        [HttpPost]
        public async Task<IActionResult> Index([Bind("CustomerID,LastAmount,Balance,DepositID")] Deposit deposit)
        {
            var userID = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                var status = 1;
                deposit.CustomerID = userID;
                if (await _depositRepository.IsIDExists(userID))
                {
                    status = 2;
                    await _depositRepository.Update(deposit);
                }
                else
                {
                    await _depositRepository.Add(deposit);
                }
                return RedirectToAction(nameof(Index), new { id = 0, status });
            }
            ViewData["DepositList"] = await _depositRepository.FindAll(userID);
            return View(deposit);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int id)
        {
            return RedirectToAction(nameof(Index), new { id = 1 });
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var site = await _depositRepository.FindByID(id);
            await _depositRepository.Remove(site);
            return RedirectToAction(nameof(Index), new { id = 0, status = 0 });
        }
    }
}
