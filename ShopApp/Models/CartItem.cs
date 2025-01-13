using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice => Product.Price * quantity;
    }
}
