using EllaJewelry.Core.Contracts;
using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EllaJewelry.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IJewellery _jewellery;
        private readonly IUser _userServices;

        public AdminController(IJewellery jewellery, IUser userServices)
        {
            _jewellery = jewellery;   
            _userServices = userServices;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Details/5
        public async Task<ActionResult> ListUsers()
        {
            IEnumerable<User> users = await _userServices.ReadAllUsersAsync();
            return View("ListUsers", users);
        }

        public async Task<ActionResult> ManageProducts()
        {
            //var products = await _jewellery.Products.ReadAllAsync();
            return RedirectToAction("List", "Product");
        }

        public async Task<ActionResult> ManageServices()
        {
            //var products = await _jewellery.Products.ReadAllAsync();
            return RedirectToAction("List", "Service");
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
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

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
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

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
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
