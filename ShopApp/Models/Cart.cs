using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class Cart
    {
        public int cart_id { get; set; }
        public List<CartItemHuy> cartItems { get; set; } = new List<CartItemHuy>();

        public int totalQuantity { get; set; }

        public DateTime orderDate { get; set; }

        public int user_id { get; set; }

        // Tính tổng giá trị của giỏ hàng (bao gồm phí giao hàng)
        public decimal totalPrice => cartItems.Sum(item => item.totalPrice);

        public decimal TotalAmount => totalPrice + 30000;

    }
}
