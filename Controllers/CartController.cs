using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Data;
using ECommerceApp.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. Ambil keranjang dari Session
            string? cartJson = HttpContext.Session.GetString("ShoppingCart");
            List<CartItem> cartSession = new List<CartItem>();
            
            if (!string.IsNullOrEmpty(cartJson))
            {
                cartSession = JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
            }

            // 2. Siapkan Nampan (ViewModel)
            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                CartDetails = new List<CartDetail>(),
                OrderTotal = 0
            };

            // 3. Cocokkan ID di Session dengan Data di Database
            var detailList = new List<CartDetail>();
            foreach (var item in cartSession)
            {
                // Tarik data asli produk dari DB
                var productFromDb = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == item.ProductId);
                
                if (productFromDb != null)
                {
                    detailList.Add(new CartDetail
                    {
                        Product = productFromDb,
                        Quantity = item.Quantity
                    });

                    // Hitung total harga: (Harga Produk x Jumlah)
                    shoppingCartVM.OrderTotal += (productFromDb.Price * item.Quantity);
                }
            }

            shoppingCartVM.CartDetails = detailList;

            // 4. Lempar nampan ke View
            return View(shoppingCartVM);
        }
    }
}