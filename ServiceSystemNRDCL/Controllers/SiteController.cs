using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            return View(await siteRepository.FindAll(_userManager.GetUserId(User)));
        }

        // GET: Sites/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var site = await siteRepository.FindByID(id);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // GET: Sites/Create
        public IActionResult Create()
        {
            var userID = _userManager.GetUserId(User);

            return View(new Site(CustomerID: userID));
        }

        // POST: Sites/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("SiteID,CustomerID,SiteName,Distance")] Site site)
        {
            if (ModelState.IsValid)
            {
                var userID = _userManager.GetUserId(User);

                site.CustomerID = userID;

                if (await siteRepository.Add(site))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(site);
        }

        // GET: Sites/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var site = await siteRepository.FindByID(id);
            if (site == null)
            {
                return NotFound();
            }
            return View(site);
        }

        // POST: Sites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SiteID,CustomerID,SiteName,Distance")] Site site)
        {
            if (id != site.SiteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await siteRepository.Update(site);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await siteRepository.IsIDExists(site.SiteID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(site);
        }

        // GET: Sites/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var site = await siteRepository.FindByID(id);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // POST: Sites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var site = await siteRepository.FindByID(id);
            if (site == null)
            {
                return NotFound();
            }

            await siteRepository.Remove(site);
            return RedirectToAction(nameof(Index));
        }
    }
}
