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
        public async Task<IActionResult> Index(int? id, int? status)
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
            ViewData["SiteList"] = await siteRepository.FindAll(userID);
            ViewBag.CustomerID = userID;
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

                site.CustomerID = userID;
                if (site.SiteID == 0)
                {
                    await siteRepository.Add(site);
                }
                else
                {
                    await siteRepository.Update(site);
                }
                return RedirectToAction(nameof(Index), new { id = 0, status });
            }
            ViewData["SiteList"] = await siteRepository.FindAll(userID);
            return View(site);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int id)
        {
            return RedirectToAction(nameof(Index), new { id });
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var site = await siteRepository.FindByID(id);
            await siteRepository.Remove(site);
            return RedirectToAction(nameof(Index), new { id = 0, status = 0 });
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
                if(siteName.Length < 3)
                {
                    return Json("Site name must have a minimum length of 3.");
                }

                if(siteName.Length > 100)
                {
                    return Json("Site name must have a maximum length of 100.");
                }
            }

            return Json(true);
        }
    }
}
