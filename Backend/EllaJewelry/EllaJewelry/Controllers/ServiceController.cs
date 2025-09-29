using EllaJewelry.Core.Contracts;
using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EllaJewelry.Web.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IJewellery _jewelry;
        public ServiceController(IJewellery jewelry)
        {
            _jewelry = jewelry;
        }

        // GET: ServiceController
        public async Task<ActionResult> List()
        {
            var services = await _jewelry.Services.ReadAllAsync();
            return View(services);
        }

        // GET: ServiceController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var service = await _jewelry.Services.ReadAsync(id);
            return View(service);
        }

        // GET: ServiceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                await _jewelry.Services.CreateAsync(service);
                return RedirectToAction(nameof(List));
            }


            return View(nameof(Create));
        }

        // GET: ServiceController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Service service;
            try
            {
                service = await _jewelry.Services.ReadAsync(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: ServiceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _jewelry.Services.UpdateAsync(service);
                    return RedirectToAction(nameof(Details), service);
                }
                catch
                {
                    // log error if needed
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }

            return RedirectToAction(nameof(List));
        }

        // GET: ServiceController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Service service;
            try
            {
                service = await _jewelry.Services.ReadAsync(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: ServiceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _jewelry.Services.DeleteAsync(id);
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
