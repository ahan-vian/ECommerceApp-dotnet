using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Models;
using ECommerceApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ECommerceApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public IActionResult Index()
    {
        var productList = _context.Products.Include(u => u.Category).ToList();
        return View(productList);
    }

    public IActionResult Details(int id)
    {
        var product = _context.Products.Include(u => u.Category).FirstOrDefault(u => u.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity)
    {
        List<CartItem> cart = new List<CartItem>();
        string? cartJson = HttpContext.Session.GetString("ShoppingCart");
        if (!string.IsNullOrEmpty(cartJson))
        {
            cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }
        var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem { ProductId = productId, Quantity = quantity });
        }

        HttpContext.Session.SetString("ShoppingCart", JsonSerializer.Serialize(cart));
        return RedirectToAction("Index");
    }
}
