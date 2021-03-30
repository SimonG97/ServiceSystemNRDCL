using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;

namespace Assignment_NRDCL.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? id, int? status)
        {
            Product product = null;
            if (id != null && id > 0)
            {
                product = await _productRepository.FindByID(id);
            }

            if (status != null && status > 0)
            {
                ViewBag.Status = true;
                ViewBag.Message = status == 1 ? "created" : "updated";
            }
            ViewData["ProductList"] = await _productRepository.FindAll();
            return View(product);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("ProductID,ProductName,Price,Rate")] Product product)
        {
            if (ModelState.IsValid)
            {
                var status = product.ProductID == 0 ? 1 : 2;
                if (product.ProductID == 0)
                {
                    await _productRepository.Add(product);
                }
                else
                {
                    await _productRepository.Update(product);
                }
                return RedirectToAction(nameof(Index), new { id = 0, status });
            }
            ViewData["ProductList"] = await _productRepository.FindAll();
            return View(product);
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
            await _productRepository.Remove(id);
            return RedirectToAction(nameof(Index), new { id = 0, status = 0 });
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDetails(int productID)
        {
            var result = await _productRepository.FindByID(productID);
            return new JsonResult(result);
        }
    }
}
