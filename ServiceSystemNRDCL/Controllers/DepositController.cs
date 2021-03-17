using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;

namespace ServiceSystemNRDCL.Controllers
{
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
            ViewData["DepositList"] = await _depositRepository.FindAll(userID);
            ViewBag.CustomerID = userID;
            return View(deposit);
        }

        // POST: Sites/Create
        [HttpPost]
        public async Task<IActionResult> Index([Bind("CustomerID,LastAmount,Balance,DepositID")] Deposit deposit)
        {
            var userID = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                var status = deposit.DepositID == 0 ? 1 : 2;

                deposit.CustomerID = userID;
                if (deposit.DepositID == 0)
                {
                    await _depositRepository.Add(deposit);
                }
                else
                {
                    await _depositRepository.Update(deposit);
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
