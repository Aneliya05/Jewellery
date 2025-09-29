using EllaJewelry.Core.Contracts;
using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EllaJewelry.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IJewellery _jewelry;
        public ProductController(IJewellery jewelry)
        {
            _jewelry = jewelry;   
        }
        // GET: ProductController
        public async Task<ActionResult> List()
        {
            var products = await _jewelry.Products.ReadAllAsync();
            return View(products);  
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var product = _jewelry.Products.ReadAsync(id);
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _jewelry.Products.CreateAsync(product);
                return RedirectToAction(nameof(List));
            }


            return View(nameof(Create));
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Product product;
            try
            {
                product = await _jewelry.Products.ReadAsync(id);
            }
            catch(ArgumentException)
            {
                return NotFound();
            }

            var categories = await _jewelry.Categories.ReadAllAsync();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", product.CategoryID);

            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _jewelry.Products.UpdateAsync(product);
                    return RedirectToAction(nameof(Details), product);
                }
                catch
                {
                    // log error if needed
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }

            // repopulate category dropdown if validation fails
            var categories = await _jewelry.Categories.ReadAllAsync();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", product.CategoryID);

            return RedirectToAction(nameof(List));
        }


        // GET: ProductController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Product product;
            try
            {
                product = await _jewelry.Products.ReadAsync(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _jewelry.Products.DeleteAsync(id);
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
