using EllaJewelry.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EllaJewelry.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IJewellery _jewelry;
        public CategoryController(IJewellery jewelry)
        {
            _jewelry = jewelry;
        }

        // GET: CategoryController
        public async Task<ActionResult> List()
        {
            var categories = await _jewelry.Categories.ReadAllAsync();
            return View(categories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var category = _jewelry.Categories.ReadAsync(id);
            return View("Admin/Product/Details", category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
