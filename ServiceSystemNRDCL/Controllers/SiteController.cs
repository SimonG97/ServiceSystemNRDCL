using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Controllers
{
    [Authorize]
    public class SiteController : Controller
    {
        private readonly ISiteRepository siteRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public SiteController(ISiteRepository siteRepository, UserManager<ApplicationUser> userManager)
        {
            this.siteRepository = siteRepository;
            _userManager = userManager;
        }

        // GET: Sites
        public async Task<IActionResult> Index(int? id, int? status, string? customerID)
        {
            var userID = _userManager.GetUserId(User);
            Site site = null;
            if (id != null && id > 0)
            {
                site = await siteRepository.FindByID(id);
            }

            if (status != null && status > 0)
            {
                ViewBag.Status = true;
                ViewBag.Message = status == 1 ? "created" : "updated";
            }
            ViewData["SiteList"] = User.IsInRole("Admin") ? await siteRepository.FindAll() :
                await siteRepository.FindAll(userID);
            ViewBag.CustomerID = string.IsNullOrEmpty(customerID) ? userID : customerID;
            return View(site);
        }

        // POST: Sites/Create
        [HttpPost]
        public async Task<IActionResult> Index([Bind("SiteID,CustomerID,SiteName,Distance")] Site site)
        {
            var userID = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                var status = site.SiteID == 0 ? 1 : 2;
                site.CustomerID = User.IsInRole("Admin") && userID.Equals(site.CustomerID) ? userID : site.CustomerID;
                if (site.SiteID == 0)
                {
                    await siteRepository.Add(site);
                }
                else
                {
                    await siteRepository.Update(site);
                }
                return RedirectToAction(nameof(Index), new { id = 0, status, customerID = "" });
            }
            ViewData["SiteList"] = await siteRepository.FindAll(userID);
            return View(site);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int id, string customerID)
        {
            return RedirectToAction(nameof(Index), new { id, customerID });
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var site = await siteRepository.FindByID(id);
            await siteRepository.Remove(site);
            return RedirectToAction(nameof(Index), new { id = 0, status = 0, customerID = "" });
        }

        public async Task<IActionResult> VerifyCustomerCID(string customerID)
        {
            if (User.IsInRole("Admin"))
            {
                //await _context.Sites.AnyAsync(e => e.SiteID == id);
                string customerCID = (await _userManager.FindByIdAsync(customerID))?.Id;
                if (string.IsNullOrEmpty(customerCID))
                {
                    ViewBag.CustomerID = _userManager.GetUserId(User);
                    return Json("Invalid customer CID. Please enter valid registered customer CID.");
                }
                return Json(true);
            }
            else
            {
                ViewBag.CustomerID = _userManager.GetUserId(User);
                return Json("Only admin user is allowed to change the customer CID.");
            }
        }

        /// <summary>
        /// To verify the site name min and max length using [Remote] attribute.
        /// </summary>
        /// <param name="siteName">Name of the site</param>
        /// <returns></returns>
        public IActionResult VerifySiteName(string siteName)
        {
            if (!string.IsNullOrEmpty(siteName))
            {
                if (siteName.Length < 3)
                {
                    return Json("Site name must have a minimum length of 3.");
                }

                if (siteName.Length > 100)
                {
                    return Json("Site name must have a maximum length of 100.");
                }
            }
            return Json(true);
        }
    }
}
