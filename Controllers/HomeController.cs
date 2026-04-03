using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Models;
using ECommerceApp.Data;
using Microsoft.EntityFrameworkCore;

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
        var productList = _context.products.Include(u => u.Category).ToList();
        return View(productList);
    }

    public IActionResult Details(int id)
    {
        var product = _context.products.Include(u => u.Category).FirstOrDefault(u=> u.Id == id);
        if(product == null)
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
}
