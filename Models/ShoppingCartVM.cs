using System.Collections.Generic;

// Ubah baris ini menjadi Models
namespace ECommerceApp.Models 
{
    public class ShoppingCartVM
    {
        public IEnumerable<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
        public decimal OrderTotal { get; set; }
    }

    public class CartDetail
    {
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}