using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.ViewModel
{

    public class Order
{
    public string DeliveryAddress { get; set; }
    public int PaymentMethod { get; set; } // 0: ShopeePay, 1: Thanh toán trực tiếp
    public List<OrderItem> CartItems { get; set; }
    public decimal TotalAmount { get; set; }
}

public class OrderItem
{
    public string FoodName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
}

}