using EllaJewelry.Core.Contracts;
using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<ActionResult> Details(int id)
        {
            var category = await _jewelry.Categories.ReadAsync(id);
            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                await _jewelry.Categories.CreateAsync(category);
                return RedirectToAction(nameof(List));
            }
            

            return View(nameof(Create));
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ProductCategory category;
            try
            {
                category = await _jewelry.Categories.ReadAsync(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ProductCategory category)
        {
            if(id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _jewelry.Categories.UpdateAsync(category);
                    return RedirectToAction(nameof(Details), category);
                }
                catch
                {
                    // log error if needed
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
           
            return RedirectToAction(nameof(List));
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            ProductCategory category;
            try
            {
                category = await _jewelry.Categories.ReadAsync(id);
            }
            catch(ArgumentException)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _jewelry.Categories.DeleteAsync(id);
            }
            catch
            {
                // log error if needed
                ModelState.AddModelError("", "Deletion was unsuccessful.");
            }

            return RedirectToAction(nameof(List));
        }
    }
}
