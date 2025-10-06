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
            var products = await _jewelry.Products.ReadAllAsync(true);
            return View(products);  
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var product = await _jewelry.Products.ReadAsync(id, true);
            return View(product);
        }

        // GET: ProductController/Create
        public async Task <ActionResult> Create()
        {
            var categories = await _jewelry.Categories.ReadAllAsync(); 

            ViewBag.CategoryID = new SelectList(categories, "Id", "Name");

            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if(imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var filePath = Path.Combine(uploads, fileName);

                    using(var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/images/" + fileName;
                }
                await _jewelry.Products.CreateAsync(product);
                await _jewelry.Images.AddImages(product.ID, product.ImageUrl);
                return RedirectToAction(nameof(List));
            }
            var categories = await _jewelry.Categories.ReadAllAsync();
            ViewBag.CategoryID = new SelectList(categories, "Id", "Name", product.CategoryID);

            return View(nameof(Create));
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Product product;
            try
            {
                product = await _jewelry.Products.ReadAsync(id, true);
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
        public async Task<ActionResult> Edit(int id, Product product, IFormFile imageFile)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var image = await _jewelry.Images.ShowImageThumbnail(id);
                    await _jewelry.Images.DeleteImage(product.ID, image.Id);
                    if (imageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(imageFile.FileName);
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                        if (!Directory.Exists(uploads))
                            Directory.CreateDirectory(uploads);

                        var filePath = Path.Combine(uploads, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        product.Images.Add(new ProductImage
                        {
                            ImageUrl = "/images/" + fileName
                        });
                    }


                    await _jewelry.Products.UpdateAsync(product);
                    await _jewelry.Images.AddImages(product.ID, product.ImageUrl);
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
                product = await _jewelry.Products.ReadAsync(id, true);
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
