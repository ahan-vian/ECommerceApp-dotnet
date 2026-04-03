using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace ECommerceApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _contex;
        public ProductController(AppDbContext context)
        {
            _contex = context;
        }

        public IActionResult Index()
        {
            var objProductList = _contex.Products.Include(p => p.Category).ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _contex.Categories.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.CategoryList = CategoryList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            if (obj.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Silahkan Pilih Kategori");
            }

            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                _contex.Products.Add(obj);
                _contex.SaveChanges();
            }

            ViewBag.CategoryList = _contex.Categories.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }
    }

}