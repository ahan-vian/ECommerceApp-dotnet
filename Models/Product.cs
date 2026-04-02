using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models
{
    public class Product
    {
        public int Id{get; set;}

        [Required]
        public string Name {get; set;} = null!;
        public string Description {get; set;} = null!;
        public decimal Price {get; set;}
        public int CategoryId {get; set;}
        public Category Category {get; set;} = null!;
    }
}